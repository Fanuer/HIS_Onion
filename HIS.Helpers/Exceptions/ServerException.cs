using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HIS.Helpers.Exceptions
{
    /// <summary>
    /// Standard Exception, if the ManagementApi receives an error from the server
    /// </summary>
    public class ServerException : Exception
    {
        #region Field
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a Exception Object after receiving an unsuccessful http Status code
        /// </summary>
        /// <param name="response">Server Response</param>
        public ServerException(HttpResponseMessage response)
        {
            if (response == null) { throw new ArgumentNullException(nameof(response)); }
            this.Response = response;
            Initialise();
        }
        #endregion

        #region Methods

        private void Initialise()
        {
            var httpErrorObject = this.Response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

            try
            {
                // Deserialize:
                var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);
                // Sometimes, there may be Model Errors:
                if (deserializedErrorObject != null)
                {
                    if (deserializedErrorObject.ModelState != null)
                    {
                        var errors = deserializedErrorObject.ModelState.Select(kvp => string.Join(". ", kvp.Value));
                        for (int i = 0; i < errors.Count(); i++)
                        {
                            // Wrap the errors up into the base Exception.Data Dictionary:
                            this.Data.Add(i, errors.ElementAt(i));
                        }
                    }
                    // Othertimes, there may not be Model Errors:
                    else
                    {
                        var error = JsonConvert.DeserializeObject<Dictionary<string, IList<string>>>(httpErrorObject);

                        foreach (var kvp in error)
                        {
                            // Wrap the errors up into the base Exception.Data Dictionary:
                            this.Data.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                else
                {
                    this.Data.Add("Error", "An unknown error has occured");
                }
            }
            catch (JsonReaderException)
            {
                throw new HttpRequestException(this.Response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Server Response
        /// </summary>
        public HttpResponseMessage Response { get; set; }

        /// <summary>
        /// Response Status Code
        /// </summary>
        public HttpStatusCode StatusCode => this.Response.StatusCode;

        /// <summary>
        /// Response Server-Messages
        /// </summary>
        public IEnumerable<string> Errors => this.Data.Values.Cast<string>().ToList();

        #endregion


    }
}
