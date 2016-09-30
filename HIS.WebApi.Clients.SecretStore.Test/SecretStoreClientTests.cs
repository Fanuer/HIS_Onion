using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Logging;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Tests;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
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
            using (IClientService clientService = new ClientService(_moqLogger, BaseUri))
            {
                var client = await clientService.AddAsync(TestClient);
                Assert.IsNotNull(client);

                var foundClient = await clientService.FindAsync(client.Id);
                Assert.IsNotNull(foundClient);
                Assert.AreEqual(client, foundClient);

                await clientService.RemoveAsync(foundClient.Id);
                try
                {
                    foundClient = await clientService.FindAsync(client.Id);
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
            using (IClientService clientService = new ClientService(_moqLogger, BaseUri))
            {
                try
                {
                    var client = await clientService.FindAsync(clientId);
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
            using (IClientService clientService = new ClientService(_moqLogger, BaseUri))
            {
                try
                {
                    client = await clientService.AddAsync(TestClient);
                    client = await clientService.AddAsync(TestClient);
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
                        await clientService.RemoveAsync(client.Id);
                    }
                }
            }
        }

        #endregion

        #region PROPERTIES

        #endregion
    }
}
