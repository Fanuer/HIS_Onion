using System;
using System.Threading.Tasks;
using HIS.WebApi.SecretStore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIS.WebApi.SecretStore.Data;
using MongoDB.Driver;

namespace HIS.WebApi.SecretStore.Test
{
    [TestClass]
    public class ClientMongoDbRepositoryTests
    {
        #region CONST

        private const string MongoDbConnectionString_Valid = "mongodb://localhost:27017";
        private const string MongoDbConnectionString_Invalid = "WRONG_CONNECTIONSTING";
        private const string TestClientName = "TestClient";

        private readonly Client _newClient = new Client(TestClientName);

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        #endregion

        #region METHODS

        [TestMethod]
        public void IsRepositoryAvailable()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                Assert.IsNotNull(rep);
            }
        }

        [TestMethod]
        public void ErrorOnInvalidConnectionString()
        {
            ISecretRepository rep = null;
            try
            {
                rep = new MongoDbSecretRepository(MongoDbConnectionString_Invalid);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is MongoConfigurationException);
            }
            finally
            {
                rep?.Dispose();
            }
        }

        [TestMethod]
        public void ErrorOnNoConnectionString()
        {
            ISecretRepository rep = null;
            try
            {
                rep = new MongoDbSecretRepository();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentNullException);
            }
            finally
            {
                rep?.Dispose();
            }
        }

        [TestMethod]
        public async Task FindAddRemoveClientAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                try
                {
                    var client = await rep.FindClientByNameAsync(TestClientName);
                    Assert.IsNull(client);

                    var addedClient = await rep.AddClientAsync(TestClientName, _newClient.AllowedOrigin, _newClient.ApplicationType, _newClient.RefreshTokenLifeTime);
                    Assert.AreEqual(addedClient.Name, _newClient.Name);
                    Assert.AreEqual(addedClient.AllowedOrigin, _newClient.AllowedOrigin);
                    Assert.AreEqual(addedClient.ApplicationType, _newClient.ApplicationType);
                    Assert.AreEqual(addedClient.RefreshTokenLifeTime, _newClient.RefreshTokenLifeTime);
                    Assert.IsTrue(addedClient.Active);

                    client = await rep.FindClientAsync(addedClient.Id);
                    Assert.IsNotNull(client);
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

        [TestMethod]
        public async Task AddClientWithoutNameCausesExceptionAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                try
                {
                    await rep.AddClientAsync("");
                    Assert.Fail();
                }
                catch (ArgumentNullException e)
                {

                }
            }
        }

        [TestMethod]
        public async Task AddClientDublicateNameCausesExceptionAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                Client client = null;
                try
                {
                    client = await rep.AddClientAsync(_newClient.Name);
                    await rep.AddClientAsync(_newClient.Name);

                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e is ArgumentException);
                }
                finally
                {
                    if (client != null)
                    {
                        await rep.RemoveClientAsync(client.Id);
                    }
                }
            }
        }

        [TestMethod]
        public async Task FindClientNoIdCausesExceptionAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                try
                {
                    var client = await rep.FindClientAsync("");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e is ArgumentNullException);
                }
            }
        }

        [TestMethod]
        public async Task FindClientNoNameCausesExceptionAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                try
                {
                    var client = await rep.FindClientByNameAsync("");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e is ArgumentNullException);
                }
            }
        }

        [TestMethod]
        public async Task RemoveClientNoIdCausesExceptionAsync()
        {
            using (var rep = new MongoDbSecretRepository(MongoDbConnectionString_Valid))
            {
                try
                {
                    await rep.RemoveClientAsync("");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e is ArgumentNullException);
                }
            }
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
