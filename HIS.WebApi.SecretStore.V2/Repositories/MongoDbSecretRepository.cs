using System;
using System.Threading.Tasks;
using HIS.WebApi.SecretStore.Data;
using HIS.WebApi.SecretStore.Data.Enums;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HIS.WebApi.SecretStore.V2.Repositories
{
    /// <summary>
    /// A Client Store which uses MongoDb
    /// </summary>
    internal class MongoDbSecretRepository: ISecretRepository
    {
        #region CONST
        private const string ClientDbName = "HIS_CLIENTS";
        private const string ClientCollectionName = "clients";
        #endregion

        #region FIELDS

        private MongoClient _dbClient;
        private IMongoDatabase _database;
        private IMongoCollection<Client> _clients;
        #endregion

        #region CTOR

        /// <summary>
        /// Creates a new MongoDbSecretRepository object
        /// </summary>
        /// <param name="options">provides options to connect to the MongoDB</param>
        public MongoDbSecretRepository(IOptions<MongoDbOptions> options)
        {
            if (options == null) { throw new ArgumentNullException(nameof(options)); }

            var connection = options.Value.ConnectionString;
            if (String.IsNullOrEmpty(connection)){throw new ArgumentNullException(nameof(options.Value.ConnectionString));}

            var dbName = options.Value.DatabaseName ?? ClientDbName;
            var collectionName = options.Value.CollectionName ?? ClientCollectionName;

            _dbClient = new MongoClient(connection);
            _database = _dbClient.GetDatabase(dbName);
            _clients = _database.GetCollection<Client>(collectionName);
            if (_clients == null)
            {
                _database.CreateCollection(ClientCollectionName);
            }
            _clients = _database.GetCollection<Client>(ClientCollectionName);
        }
        
        #endregion

        #region METHODS
        
        /// <summary>
        /// Add a new Client to the Database
        /// </summary>
        /// <param name="name">Client name</param>
        /// <param name="allowOrigin">allowed origins</param>
        /// <param name="type">Application Type</param>
        /// <param name="timeSpan">Type Span in Month</param>
        /// <returns></returns>
        public async Task<Client> AddClientAsync(string name, string allowOrigin ="*", ApplicationType type = ApplicationType.JavaScript, int timeSpan = 6)
        {
            if (String.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name));}

            var oldClient = await this.FindClientByNameAsync(name);
            if (oldClient != null)
            {
                throw new ArgumentException("A client with the given name already exists");
            }

            var client = new Client(name, allowOrigin)
            {
                 ApplicationType= type,
                 RefreshTokenLifeTime = timeSpan
            };
            await this._clients.InsertOneAsync(client);
            return client;
        }

        /// <summary>
        /// Removes the given client
        /// </summary>
        /// <param name="clientId">Id of the Client</param>
        /// <returns></returns>
        public async Task RemoveClientAsync(string clientId)
        {
            if (String.IsNullOrEmpty(clientId)) { throw new ArgumentNullException(nameof(clientId)); }
            await this._clients.FindOneAndDeleteAsync(x => clientId.Equals(x.Id));
        }

        /// <summary>
        /// Find a client by its id
        /// </summary>
        /// <param name="clientId">Id of a client</param>
        /// <returns></returns>
        public async Task<Client> FindClientAsync(string clientId)
        {
            if (String.IsNullOrEmpty(clientId)) { throw new ArgumentNullException(nameof(clientId)); }
            return await this._clients.Find(x => clientId.Equals(x.Id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Find a client by its name
        /// </summary>
        /// <param name="clientName">name of a client</param>
        /// <returns></returns>
        public async Task<Client> FindClientByNameAsync(string clientName)
        {
            if (String.IsNullOrEmpty(clientName)) { throw new ArgumentNullException(nameof(clientName)); }
            return await this._clients.Find(x => clientName.Equals(x.Name)).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Disposes this Object
        /// </summary>
        public void Dispose()
        {
            _clients = null;
            _database = null;
            _dbClient = null;
        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
