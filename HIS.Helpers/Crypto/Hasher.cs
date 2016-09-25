using System;
using System.Text;
using Org.BouncyCastle.Security;

namespace HIS.Helpers.Crypto
{
    /// <summary>
    /// Hasher Implementation from http://www.thomas-weller.de/en/a-secure-password-hashing-implementation-for-pcls/
    /// </summary>
    public class Hasher
    {
        #region CONST
        #endregion

        #region FIELDS
        private static Hasher _current = null;
        private int _saltByteSize;
        private int _hashByteSize;
        private int _pbkdf2Iterations;
        private const char Delimiter = ':';
        #endregion

        #region CTOR
        /// <summary>
        /// Intitialises the properties
        /// Sets defaults: 64 for <see cref="HashByteSize"/> and <see cref="SaltByteSize"/>, 1000 for <see cref="Pbkdf2Iterations"/>.
        /// </summary>
        protected Hasher()
        {
            this.HashByteSize = 64;
            this.SaltByteSize = 64;
            this.Pbkdf2Iterations = 1000;
        }
        #endregion

        #region METHODS
        private byte[] CreateRandomSalt()
        {
            var random = SecureRandom.GetInstance("SHA256PRNG", true);

            var salt = new byte[SaltByteSize];
            random.NextBytes(salt);

            return salt;
        }

        /// <summary>
        /// Creates a salted Pbkdf2 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public string CreateHash(string password)
        {
            lock (this)
            {
                var salt = CreateRandomSalt();

                // Hash the password and encode the parameters
                var  hash = new Pbkdf2().GenerateDerivedKey(HashByteSize, Encoding.UTF8.GetBytes(password), salt, Pbkdf2Iterations);
                return $"{Pbkdf2Iterations}{Delimiter}{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
            }
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool ValidatePassword(string password, string correctHash)
        {
            string[] hashParts = correctHash.Split(Delimiter);
            if (hashParts.Length <3)
            {
                return false;
            }
            int iterations = Int32.Parse(hashParts[0]);
            byte[] salt = Convert.FromBase64String(hashParts[1]);
            byte[] hash = Convert.FromBase64String(hashParts[2]);

            byte[] testHash = new Pbkdf2().GenerateDerivedKey(hash.Length, Encoding.UTF8.GetBytes(password), salt, iterations);

            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. false otherwise.</returns>
        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Length (in bytes) of the password salt.
        /// </summary>
        /// <remarks>
        /// Can be between 32 and 128, default is 64.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set a value less than 32 or greater than 128.
        /// </exception>
        public int SaltByteSize
        {
            get { return _saltByteSize; }
            set
            {
                if (value < 32 || value > 128)
                {
                    throw new ArgumentException("Value must be between 12 and 36.");
                }
                _saltByteSize = value;
            }
        }

        /// <summary>
        /// Length (in bytes) of the generated hash.
        /// </summary>
        /// <remarks>
        /// Can be between 32 and 128, default is 64.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set a value less than 32 or greater than 128.
        /// </exception>
        public int HashByteSize
        {
            get { return _hashByteSize; }
            set
            {
                if (value < 32 || value > 128)
                {
                    throw new ArgumentException("Value must be between 12 and 36.");
                }
                _hashByteSize = value;
            }
        }

        /// <summary>
        /// Number of iterations for the hashing algorithm.
        /// </summary>
        /// <remarks>
        /// Can be between 500 and 2000, default is 1000.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set a value less than 500 or greater than 2000.
        /// </exception>
        public int Pbkdf2Iterations
        {
            get { return _pbkdf2Iterations; }
            set
            {
                if (value < 500 || value > 2000)
                {
                    throw new ArgumentException("Value must be between 500 and 2000.");
                }
                _pbkdf2Iterations = value;
            }
        }
        /// <summary>
        /// Grants access to the Singleton Instance
        /// </summary>
        public static Hasher Current => _current ?? (_current = new Hasher());
        #endregion
    }
}
