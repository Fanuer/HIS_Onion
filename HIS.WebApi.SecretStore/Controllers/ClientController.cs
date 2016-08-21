using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HIS.WebApi.SecretStore.Data;
using HIS.WebApi.SecretStore.Repositories;
using Swashbuckle.Swagger.Annotations;

namespace HIS.WebApi.SecretStore.Controllers
{
    /// <summary>
    /// Gets access to Client Store
    /// </summary>
    [RoutePrefix("api/clients")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    public class ClientController : ApiController
    {
        /// <summary>
        /// Find a Client by its Id
        /// </summary>
        /// <param name="clientId">Id if the Client</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}", Name = "GetClientById")]
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> FindClientAsync(string clientId)
        {
            Client result;
            using (ISecretRepository rep = new MongoDbSecretRepository())
            {
                result = await rep.FindClientAsync(clientId);
            }
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Adds a Client to use the Auth Server
        /// </summary>
        /// <param name="clientModel">Client Data</param>
        /// <returns></returns>
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(ClientViewModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateAsync([FromBody]ClientViewModel clientModel)
        {
            Client client;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (ISecretRepository rep = new MongoDbSecretRepository())
            {
                client = await rep.AddClientAsync(clientModel.Name);
            }

            return CreatedAtRoute("GetClientById", new { clientId = client.Id }, client);
        }
        
        /// <summary>
        /// Removes a Client
        /// </summary>
        /// <param name="clientId">Id of the client to delete</param>
        /// <returns></returns>
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{clientId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteClientAsync(string clientId)
        {
            Client client;
            using (ISecretRepository rep = new MongoDbSecretRepository())
            {
                client = await rep.FindClientAsync(clientId);
                if (client== null)
                {
                    return NotFound();
                }
                await rep.RemoveClientAsync(clientId);
            }
            return Ok();
        }
    }
}
