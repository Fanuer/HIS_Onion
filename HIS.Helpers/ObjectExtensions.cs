using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HIS.Helpers
{
    public static class ObjectExtensions
    {
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
