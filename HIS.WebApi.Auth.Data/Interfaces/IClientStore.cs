using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Auth.Data.Interfaces
{
    public interface IClientStore : IDisposable
    {
        /// <summary>
        /// Returns a Client by its id
        /// </summary>
        /// <param name="clientId">Id of the client</param>
        /// <returns></returns>
        Task<Client> FindClientAsync(string clientId);
        /// <summary>
        /// Create a new Client
        /// </summary>
        /// <param name="clientName">Name of the client</param>
        /// <returns></returns>
        Task<Client> CreateClientAsync(string clientName);
        /// <summary>
        /// Removes a Client
        /// </summary>
        /// <param name="clientId">Id of the client</param>
        /// <returns></returns>
        Task DeleteClientAsync(string clientId);
    }
}
