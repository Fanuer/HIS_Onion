using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;

namespace HIS.Helpers
{
    public static class ObjectExtensions
    {
        public static string GetShaHash(this string input)
        {
            if (String.IsNullOrEmpty(input)){ throw new ArgumentNullException(input); }

            var encData = Encoding.UTF8.GetBytes(input);
            var myHash = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            myHash.BlockUpdate(encData, 0, encData.Length);
            var compArr = new byte[myHash.GetDigestSize()];
            myHash.DoFinal(compArr, 0);

            return Convert.ToBase64String(compArr);
        }
        

        /// <summary>
        /// Tries to deserialise the content from a JSON to an object
        /// </summary>
        /// <typeparam name="T">type of target object</typeparam>
        /// <param name="content">current http content</param>
        /// <returns></returns>
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            T result = default(T);
            var stringcontent = await content.ReadAsStringAsync();
            try
            {
                result = (T)JsonConvert.DeserializeObject(stringcontent, typeof(T));
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }

        


    }
}
