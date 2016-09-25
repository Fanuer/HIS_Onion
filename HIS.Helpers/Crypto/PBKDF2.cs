using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace HIS.Helpers.Crypto
{
    /// <summary>
    /// Found on http://stackoverflow.com/a/3213054
    /// see: https://en.wikipedia.org/wiki/PBKDF2
    /// </summary>
    internal class Pbkdf2
    {
        #region CONST
        private readonly IMac _hMac = new HMac(new Sha1Digest());

        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        private void F(
    byte[] P,
    byte[] S,
    int c,
    byte[] iBuf,
    byte[] outBytes,
    int outOff)
        {
            byte[] state = new byte[_hMac.GetMacSize()];
            var param = new KeyParameter(P);

            _hMac.Init(param);

            if (S != null)
            {
                _hMac.BlockUpdate(S, 0, S.Length);
            }

            _hMac.BlockUpdate(iBuf, 0, iBuf.Length);

            _hMac.DoFinal(state, 0);

            Array.Copy(state, 0, outBytes, outOff, state.Length);

            for (int count = 1; count != c; count++)
            {
                _hMac.Init(param);
                _hMac.BlockUpdate(state, 0, state.Length);
                _hMac.DoFinal(state, 0);

                for (int j = 0; j != state.Length; j++)
                {
                    outBytes[outOff + j] ^= state[j];
                }
            }
        }

        private void IntToOctet(
            byte[] Buffer,
            int i)
        {
            Buffer[0] = (byte)((uint)i >> 24);
            Buffer[1] = (byte)((uint)i >> 16);
            Buffer[2] = (byte)((uint)i >> 8);
            Buffer[3] = (byte)i;
        }

        // Use this function to retrieve a derived key.
        // dkLen is in octets, how much bytes you want when the function to return.
        // mPassword is the password converted to bytes.
        // mSalt is the salt converted to bytes
        // mIterationCount is the how much iterations you want to perform. 

        /// <summary>
        /// Use this function to retrieve a derived key.
        /// </summary>
        /// <param name="dkLen">octets, how much bytes you want when the function to return</param>
        /// <param name="mPassword">the password converted to bytes</param>
        /// <param name="mSalt">is the salt converted to bytes</param>
        /// <param name="mIterationCount">the number of iterations you want to perform</param>
        /// <returns></returns>
        public byte[] GenerateDerivedKey(int dkLen, byte[] mPassword, byte[] mSalt, int mIterationCount)
        {
            int hLen = _hMac.GetMacSize();
            int l = (dkLen + hLen - 1) / hLen;
            byte[] iBuf = new byte[4];
            byte[] outBytes = new byte[l * hLen];

            for (int i = 1; i <= l; i++)
            {
                IntToOctet(iBuf, i);

                F(mPassword, mSalt, mIterationCount, iBuf, outBytes, (i - 1) * hLen);
            }
            /*
             By this time outBytes will contain the derived key + more bytes.
             According to the PKCS #5 v2.0: Password-Based Cryptography Standard (www.truecrypt.org/docs/pkcs5v2-0.pdf) 
             we have to "extract the first dkLen octets to produce a derived key".

             I am creating a byte array with the size of dkLen and then using
             Buffer.BlockCopy to copy ONLY the dkLen amount of bytes to it
             And finally returning it :D
             */

            byte[] output = new byte[dkLen];

            Buffer.BlockCopy(outBytes, 0, output, 0, dkLen);

            return output;
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
