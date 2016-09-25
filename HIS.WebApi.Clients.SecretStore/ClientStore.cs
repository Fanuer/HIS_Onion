using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using HIS.Helpers.Http;
using HIS.WebApi.SecretStore.Data;

namespace HIS.WebApi.Clients.SecretStore
{
    public class ClientStore:IClientStore
    {
        #region CONST

        private const string ClientAreaName = "api/client";
        #endregion

        #region FIELDS

        private HttpConnectorBase _connector;
        #endregion

        #region CTOR

        public ClientStore(ILog logger, string baseUri)
        {
            if (String.IsNullOrEmpty(baseUri)){throw new ArgumentNullException(nameof(baseUri));}
            if (logger == null) {throw new ArgumentNullException(nameof(logger));}
            this._connector = new HttpConnectorBase(logger, baseUri);
        }
        #endregion

        #region METHODS

        public void Dispose()
        {
            this._connector.Dispose();
            this._connector = null;
        }

        public async Task<Client> FindClientAsync(string clientId)
        {
            return await _connector.GetAsync<Client>($"{ClientAreaName}/{clientId}");
        }

        public async Task<Client> CreateClientAsync(string clientName)
        {
            return await this._connector.PostAsJsonReturnAsync<object, Client>(new { Name = clientName}, $"{ClientAreaName}/");
        }

        public async Task DeleteClientAsync(string clientId)
        {
            await this._connector.DeleteAsync($"{ClientAreaName}/{clientId}");
        }
        #endregion

        #region PROPERTIES
        #endregion


    }
}
