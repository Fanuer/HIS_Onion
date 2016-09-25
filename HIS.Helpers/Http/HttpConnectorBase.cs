using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Models;
using Newtonsoft.Json;

namespace HIS.Helpers.Http
{
    /// <summary>
    /// Wrapper for HttpClient. Extends HttpClient to handle JSON-Data
    /// </summary>
    public class HttpConnectorBase : ChangableObject, IDisposable
    {
        #region CONST

        #endregion

        #region FIELDS

        private readonly ILog _logger;
        private HttpClientHandler _handler;
        private HttpClient _client;

        #endregion

        #region CTOR

        /// <summary>
        /// Creates a new instance 
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="baseUri">Http Client Base Uri</param>
        public HttpConnectorBase(ILog logger, string baseUri)
        {
            if (String.IsNullOrEmpty(baseUri)) { throw new ArgumentNullException(nameof(baseUri)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }

            this._logger = logger;
            this._handler = new HttpClientHandler();
            this._client = new HttpClient(_handler)
            {
                BaseAddress = new Uri(baseUri),
                Timeout = TimeSpan.FromMinutes(15)
            };

            this._client.DefaultRequestHeaders.Accept.Clear();
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Creates a get url parameter list of each property of the given object 
        /// </summary>
        /// <param name="data">Object with properties</param>
        /// <returns></returns>
        protected string QueryString(object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var parts = new List<string>();

            foreach (var p in data.GetType().GetRuntimeProperties().Select(x => x.GetMethod))
            {
                var value = p.Invoke(data, null);

                if (value != null)
                {
                    string sVal;
                    if (value is bool)
                    {
                        sVal = ((bool)value) ? "true" : "false";
                    }
                    else if (value is DateTime)
                    {
                        sVal = ((DateTime)value).ToString("yyyyMMdd");
                    }
                    else
                    {
                        sVal = WebUtility.UrlEncode(value.ToString());
                    }

                    parts.Add(p.Name + "=" + sVal);
                }
            }

            return parts.Count > 0 ? "?" + String.Join("&", parts) : String.Empty;
        }

        /// <summary>
        /// Releases unnessassary Resources
        /// </summary>
        public void Dispose()
        {
            // try cancel pending requests
            try { this._client.CancelPendingRequests(); } catch { }
            try { this._client.Dispose(); } catch { }
            try { this._handler.Dispose(); } catch { }

            this._client = null;
            _handler = null;

        }

        /// <summary>
        /// Creates a HTTPGET Request and converts the successfull answer into an object of the given Type
        /// </summary>
        /// <typeparam name="T">Type to convert the Answer to</typeparam>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<T> GetAsync<T>(string url, params object[] args)
        {
            var response = await this._client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new ServerException(response);
        }

        /// <summary>
        /// Return the Urls Http Status
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<HttpStatusCode> GetHttpStatusAsync(string url, params object[] args)
        {
            var response = await this._client.GetAsync(String.Format(url, args));
            return response.StatusCode;
        }

        /// <summary>
        /// Creates a HTTP-Get Request and converts the successfull answer into a byte array
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<byte[]> GetBytesAsync(string url, params object[] args)
        {

            var response = await this._client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"GetBytesAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                return await response.Content.ReadAsByteArrayAsync();
            }
            if (_logger.IsInfoEnabled) _logger.Info($"Failed GetBytesAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");

            throw new ServerException(response);
        }

        /// <summary>
        /// Creates a HTTP-Get Request and converts the successfull answer into a stream
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<StreamModel> GetStreamAsync(string url, params object[] args)
        {
            var response = await this._client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"GetStreamAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");

                var stream = new StreamModel
                {
                    Stream = await response.Content.ReadAsStreamAsync(),
                    FileName = response.Content.Headers.ContentDisposition.FileName
                };

                return stream;
            }

            if (_logger.IsInfoEnabled) _logger.Info($"Failed GetStreamAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");

            throw new ServerException(response);
        }

        /// <summary>
        /// Creates a HTTP-Get Request and converts the successfull answer into a xml stream
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<XmlStreamModel> GetXmlStreamAsync(string url, params object[] args)
        {
            var response = await this._client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"GetXmlStreamAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");

                return new XmlStreamModel
                {
                    Stream = await response.Content.ReadAsStreamAsync(),
                    FileName = response.Content.Headers.ContentDisposition.FileName
                };
            }

            if (_logger.IsInfoEnabled) _logger.Info($"Failed GetXmlStreamAsync<{typeof(byte[]).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");

            throw new ServerException(response);
        }

        /// <summary>
        /// Converts the given model to JSON and send it as an HTTP PUT Request
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="model">Data to send</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task PutAsJsonAsync<T>(T model, string url, params object[] args)
        {

            var response = await this.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PutAsJsonAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled)_logger.Info($"Failed PutAsJsonAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Creates a HTTP PUT Request to the given url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task PutAsync(string url, params object[] args)
        {
            HttpResponseMessage response = await this._client.PutAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PutAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info($"Failed PutAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Creates a HTTP DELETE Request to the given url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task DeleteAsync(string url, params object[] args)
        {

            var response = await this._client.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"DeleteAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info($"Failed DeleteAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response.ToString()}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Sends a HTTP DELETE Request and converts the response into a given type
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<T> DeleteAsync<T>(string url, params object[] args)
        {

            var response = await this._client.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"DeleteAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                return await response.Content.ReadAsAsync<T>();
            }
            if (_logger.IsInfoEnabled) _logger.Info($"Failed DeleteAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response.ToString()}");
            throw new ServerException(response);
        }

        /// <summary>
        /// Sends a HTTP POST Request and converts the response into a given type
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="content">HTTP POST Body</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<T> PostAsync<T>(HttpContent content, string url, params object[] args)
        {

            var response = await this._client.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PostAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                return await response.Content.ReadAsAsync<T>();
            }
            if (_logger.IsInfoEnabled) _logger.Info($"Failed PostAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
            throw new ServerException(response);
        }

        /// <summary>
        /// Sends a HTTP Post Request
        /// </summary>
        /// <param name="content">HTTP POST Body</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task PostAsync(HttpContent content, string url, params object[] args)
        {

            var response = await this._client.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PostAsync(HttpContent, {String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info($"Failed PostAsync(HttpContent, {String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Sends a HTTP Post Request
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task PostAsync(string url, params object[] args)
        {

            var response = await this._client.PostAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PostAsJsonAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info($"Failed PutAsJsonAsync({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Convert the given model into JSON and sends it as a HTTP POST Request
        /// </summary>
        /// <typeparam name="T">type of the model to send</typeparam>
        /// <param name="model">Post data</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task PostAsJsonAsync<T>(T model, string url, params object[] args)
        {

            var response = await this.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PostAsJsonAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                response.Content.Dispose();
            }
            else
            {
                if (_logger.IsInfoEnabled) _logger.Info($"Failed PutAsJsonAsync<{typeof(T).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
                throw new ServerException(response);
            }
        }

        /// <summary>
        /// Convert the model into a JSON Object, sends it as HTTP PUT Request and converts the response into a given type
        /// </summary>
        /// <param name="model">PUT body data</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <typeparam name="T">Type of the sending Model</typeparam>
        /// <typeparam name="TResult">Type of the response model</typeparam>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<TResult> PutAsJsonReturnAsync<T, TResult>(T model, string url, params object[] args)
        {
            var response = await this.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PutAsJsonAsyncReturn<{typeof(T).Name}, {typeof(TResult).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                return await response.Content.ReadAsAsync<TResult>();
            }

            if (_logger.IsInfoEnabled) _logger.Info($"Failed PutAsJsonAsync<{typeof(T).Name}, {typeof(TResult).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
            throw new ServerException(response);
        }

        /// <summary>
        /// Convert the model into a JSON Object, sends it as HTTP POST Request and converts the response into a given type
        /// </summary>
        /// <param name="model">POST body data</param>
        /// <param name="url">Url</param>
        /// <param name="args">Arguments to insert into the url using String.Format</param>
        /// <typeparam name="T">Type of the sending Model</typeparam>
        /// <typeparam name="TResult">Type of the response model</typeparam>
        /// <returns></returns>
        /// <exception cref="ServerException">If the response is unsuccessful, a ServerException with additional Information is created</exception>
        public async Task<TResult> PostAsJsonReturnAsync<T, TResult>(T model, string url, params object[] args)
        {
            var response = await this.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                if (_logger.IsDebugEnabled) _logger.Debug($"PostAsJsonAsyncReturn<{typeof(T).Name}, {typeof(TResult).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}");
                return await response.Content.ReadAsAsync<TResult>();
            }
            if (_logger.IsInfoEnabled) _logger.Info($"Failed PostAsJsonAsyncReturn<{typeof(T).Name}, {typeof(TResult).Name}>({String.Format(url, args)}) -> {await response.Content.ReadAsStringAsync()}{response}");
            throw new ServerException(response);
        }

        /// <summary>
        /// Serialises the model as JSON and send it as POST Request
        /// </summary>
        /// <typeparam name="T">type of model object</typeparam>
        /// <param name="url">target url</param>
        /// <param name="model">model to send</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await this._client.PostAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }

        /// <summary>
        /// Serialises the model as JSON and send it as PUT Request
        /// </summary>
        /// <typeparam name="T">type of model object</typeparam>
        /// <param name="url">target url</param>
        /// <param name="model">model to send</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await this._client.PutAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}
