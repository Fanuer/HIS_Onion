﻿using System.Net;
using System.Threading.Tasks;
using HIS.WebApi.SecretStore.Data;
using HIS.WebApi.SecretStore.V2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace HIS.WebApi.SecretStore.V2.Controllers
{
    /// <summary>
    /// Gets access to Client Store
    /// </summary>
    [Route("api/[controller]")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    public class ClientController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        private readonly ISecretRepository _repository;
        #endregion

        #region CTOR

        /// <summary>
        /// Creates a new instance of this controller
        /// </summary>
        /// <param name="repository">used repository</param>
        public ClientController(ISecretRepository repository)
        {
            this._repository = repository;
        }
      
        #endregion

        #region METHODS
        /// <summary>
        /// Find a Client by its Id
        /// </summary>
        /// <param name="clientId">Id if the Client</param>
        /// <returns></returns>
        [HttpGet("{clientId}", Name = "GetClientById")]
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IActionResult> FindClientAsync(string clientId)
        {
            var result = await _repository.FindClientAsync(clientId);
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
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([Microsoft.AspNetCore.Mvc.FromBody]ClientViewModel clientModel)
        {
            if (ModelState.IsValid)
            {
                var foundClient = await _repository.FindClientByNameAsync(clientModel.Name);
                if (foundClient != null)
                {
                    ModelState.AddModelError("Name", "A client with the given name already exists");
                }
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _repository.AddClientAsync(clientModel.Name);
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
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClientAsync(string clientId)
        {
            var client = await _repository.FindClientAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            await _repository.RemoveClientAsync(clientId);
            return Ok();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            this._repository.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}
