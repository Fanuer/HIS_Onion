using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Logging;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Tests;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HIS.WebApi.Clients.SecretStore.Test
{
    [TestClass]
    public class SecretStoreClientTests
    {
        #region CONST

        private const string TestClient = "testClient";
        private const string BaseUri = "http://localhost:52566/";
        private readonly ILog _moqLogger = new MockLogger();

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        #endregion

        #region METHODS
        [TestMethod]
        public async Task AddFindDeleteViaHttpPossible()
        {
            using (IClientStore clientStore = new ClientStore(_moqLogger, BaseUri))
            {
                var client = await clientStore.CreateClientAsync(TestClient);
                Assert.IsNotNull(client);

                var foundClient = await clientStore.FindClientAsync(client.Id);
                Assert.IsNotNull(foundClient);
                Assert.AreEqual(client, foundClient);

                await clientStore.DeleteClientAsync(foundClient.Id);
                try
                {
                    foundClient = await clientStore.FindClientAsync(client.Id);
                    Assert.Fail();
                }
                catch (ServerException e)
                {
                    Assert.AreEqual(e.StatusCode, HttpStatusCode.NotFound);
                }
            }

        }

        [TestMethod]
        public async Task FindCauses404IfElementNotExists()
        {
            const string clientId = "bla";
            using (IClientStore clientStore = new ClientStore(_moqLogger, BaseUri))
            {
                try
                {
                    var client = await clientStore.FindClientAsync(clientId);
                    Assert.Fail();
                }
                catch (ServerException e)
                {
                    Assert.AreEqual(e.StatusCode, HttpStatusCode.NotFound);
                }
            }
        }

        [TestMethod]
        public async Task CreateCauses400IfElementWithSameNameExists()
        {
            Client client = null;
            using (IClientStore clientStore = new ClientStore(_moqLogger, BaseUri))
            {
                try
                {
                    client = await clientStore.CreateClientAsync(TestClient);
                    client = await clientStore.CreateClientAsync(TestClient);
                    Assert.Fail();
                }
                catch (ServerException e)
                {
                    Assert.AreEqual(e.StatusCode, HttpStatusCode.BadRequest);
                }
                finally
                {
                    if (client != null)
                    {
                        await clientStore.DeleteClientAsync(client.Id);
                    }
                }
            }
        }

        #endregion

        #region PROPERTIES

        #endregion
    }
}
