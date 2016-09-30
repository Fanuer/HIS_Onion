using System;
using System.Threading.Tasks;
using Common.Logging;
using HIS.Helpers.Http;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Clients.SecretStore
{
    public class ClientService:IClientService
    {
        #region CONST

        private const string ClientAreaName = "api/client";
        #endregion

        #region FIELDS

        private HttpConnectorBase _connector;
        #endregion

        #region CTOR

        public ClientService(ILog logger, string baseUri)
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

        public async Task<Client> FindAsync(string id)
        {
            if (String.IsNullOrEmpty(id)) { throw new ArgumentNullException(nameof(id)); }

            return await _connector.GetAsync<Client>($"{ClientAreaName}/{id}");
        }

        public async Task<Client> AddAsync(string clientName)
        {
            var client = new Client(clientName);
            return await this.AddAsync(client);
        }

        public async Task<Client> AddAsync(Client model)
        {
            return await this._connector.PostAsJsonReturnAsync<object, Client>(new {model.Name}, $"{ClientAreaName}/");
        }

        public async Task<bool> RemoveAsync(string id)
        {
            if (String.IsNullOrEmpty(id)){throw new ArgumentNullException(nameof(id));}

            await this._connector.DeleteAsync($"{ClientAreaName}/{id}");
            return true;
        }

        public async Task<bool> RemoveAsync(Client model)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            if (String.IsNullOrEmpty(model.Id)) { throw new ArgumentNullException(nameof(model.Id), "Id Property of given model must be set"); }

            await this._connector.DeleteAsync($"{ClientAreaName}/{model.Id}");
            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            if (String.IsNullOrEmpty(id)) { throw new ArgumentNullException(nameof(id)); }
            var client = await FindAsync(id);
            return client != null;
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}
