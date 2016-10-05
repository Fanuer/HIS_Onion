using System;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Core.Models;
using HIS.WebApi.Auth.Data.Core.Models.Enums;

namespace HIS.WebApi.SecretStore.V2.Repositories
{
    public interface IClientRepository: IDisposable
    {
        Task<Client> AddClientAsync(string name, string allowOrigin = "*", ApplicationType type = ApplicationType.JavaScript, int timeSpan = 6);
        Task RemoveClientAsync(string clientId);
        Task<Client> FindClientAsync(string clientId);
        Task<Client> FindClientByNameAsync(string clientName);
    }
}
