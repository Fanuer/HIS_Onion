using System;
using System.Threading.Tasks;
using HIS.Helpers.WebApi.Options;
using HIS.WebApi.Auth.Data.Models;
using HIS.WebApi.SecretStore.V2.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace HIS.WebApi.SecretStore.V2.Test
{
    public class ClientMongoDbRepositoryTests
    {
        #region CONST

        private const string MongoDbConnectionString_Valid = "mongodb://localhost:27017";
        private const string MongoDbConnectionString_Invalid = "WRONG_CONNECTIONSTING";
        private const string TestClientName = "TestClient";

        private readonly Client _newClient = new Client(TestClientName);
        
        private readonly IOptions<MongoDbOptions> validOptions = new OptionsWrapper<MongoDbOptions>(new MongoDbOptions() { ConnectionString = MongoDbConnectionString_Valid });
        private readonly IOptions<MongoDbOptions> noConnStringOptions = new OptionsWrapper<MongoDbOptions>(new MongoDbOptions());
        private readonly IOptions<MongoDbOptions> invalidOptions = new OptionsWrapper<MongoDbOptions>(new MongoDbOptions() { ConnectionString = MongoDbConnectionString_Invalid });

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        #endregion

        #region METHODS

        [Fact]
        public void IsRepositoryAvailable()
        {
            using (var rep = new MongoDbClientRepository(validOptions))
            {
                Assert.NotNull(rep);
            }
        }

        [Fact]
        public void ErrorOnInvalidConnectionString()
        {
            IClientRepository rep = null;
            Assert.Throws<MongoConfigurationException>(() => { rep = new MongoDbClientRepository(invalidOptions); });
            rep?.Dispose();
        }

        [Fact]
        public void ErrorOnNoOptions()
        {
            IClientRepository rep = null;
            Assert.Throws<ArgumentNullException>(() => { rep = new MongoDbClientRepository(null); });
            rep?.Dispose();
        }

        [Fact]
        public void ErrorOnNoConnectionString()
        {
            IClientRepository rep = null;
            Assert.Throws<ArgumentNullException>(() => { rep = new MongoDbClientRepository(noConnStringOptions); });
            rep?.Dispose();
        }

        [Fact]
        public async Task FindAddRemoveClientAsync()
        {
            using (var rep = new MongoDbClientRepository(validOptions))
            {
                try
                {
                    var client = await rep.FindClientByNameAsync(TestClientName);
                    Assert.Null(client);

                    var addedClient = await rep.AddClientAsync(TestClientName, _newClient.AllowedOrigin, _newClient.ApplicationType, _newClient.RefreshTokenLifeTime);
                    Assert.Equal(addedClient.Name, _newClient.Name);
                    Assert.Equal(addedClient.AllowedOrigin, _newClient.AllowedOrigin);
                    Assert.Equal(addedClient.ApplicationType, _newClient.ApplicationType);
                    Assert.Equal(addedClient.RefreshTokenLifeTime, _newClient.RefreshTokenLifeTime);
                    Assert.True(addedClient.Active);

                    client = await rep.FindClientAsync(addedClient.Id);
                    Assert.NotNull(client);
                }
                finally
                {
                    var client = await rep.FindClientByNameAsync(TestClientName);
                    if (client != null)
                    {
                        await rep.RemoveClientAsync(client.Id);
                    }
                }
            }
        }

        [Fact]
        public async Task AddClientWithoutNameCausesExceptionAsync()
        {
            using (IClientRepository rep = new MongoDbClientRepository(validOptions))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await rep?.AddClientAsync(""));
            }
        }

        [Fact]
        public async Task AddClientDublicateNameCausesExceptionAsync()
        {
            using (IClientRepository rep = new MongoDbClientRepository(validOptions))
            {
                Client client =  await rep.AddClientAsync(_newClient.Name);
                await Assert.ThrowsAsync<ArgumentException>(async () => await rep?.AddClientAsync(_newClient.Name));
                if (client != null)
                {
                    await rep.RemoveClientAsync(client.Id);
                }
            }
        }

        [Fact]
        public async Task FindClientNoIdCausesExceptionAsync()
        {
            using (IClientRepository rep = new MongoDbClientRepository(validOptions))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await rep.FindClientAsync(""));
            }
        }

        [Fact]
        public async Task FindClientNoNameCausesExceptionAsync()
        {
            using (IClientRepository rep = new MongoDbClientRepository(validOptions))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await rep.FindClientByNameAsync(""));
            }
        }

        [Fact]
        public async Task RemoveClientNoIdCausesExceptionAsync()
        {
            using (IClientRepository rep = new MongoDbClientRepository(validOptions))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await rep.RemoveClientAsync(""));
            }
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
