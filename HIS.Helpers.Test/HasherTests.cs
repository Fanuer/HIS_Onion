using System;
using HIS.Helpers.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HIS.Helpers.Test
{
    [TestClass]
    public class HasherTests
    {
        [TestMethod]
        public void CanValidatePasswordAgainstDifferntHashes()
        {
            var password = "blah123XY";
            string hash1 = Hasher.Current.CreateHash(password);
            string hash2 = Hasher.Current.CreateHash(password);

            Assert.AreNotEqual(hash1, hash2);
            Assert.IsTrue(Hasher.Current.ValidatePassword(password, hash1));
            Assert.IsTrue(Hasher.Current.ValidatePassword(password, hash2));
        }

        [TestMethod]
        public void CanValidateAnyHashWithAnySettings()
        {
            const string password = "blah123XY";

            // set hash creation values, then create hash
            Hasher.Current.SaltByteSize = 75;
            Hasher.Current.HashByteSize = 48;
            Hasher.Current.Pbkdf2Iterations = 1500;

            string hash = Hasher.Current.CreateHash(password);

            // now set different values
            Hasher.Current.SaltByteSize = 122;
            Hasher.Current.HashByteSize = 99;
            Hasher.Current.Pbkdf2Iterations = 500;

            // assert that the hash validates anyway
            Assert.IsTrue(Hasher.Current.ValidatePassword(password, hash));
        }
    }
}
