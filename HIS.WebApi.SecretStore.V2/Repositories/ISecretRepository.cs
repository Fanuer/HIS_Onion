using System;
using System.Threading.Tasks;
using HIS.WebApi.SecretStore.Data;
using HIS.WebApi.SecretStore.Data.Enums;

namespace HIS.WebApi.SecretStore.V2.Repositories
{
    public interface ISecretRepository: IDisposable
    {
        Task<Client> AddClientAsync(string name, string allowOrigin = "*", ApplicationType type = ApplicationType.JavaScript, int timeSpan = 6);
        Task RemoveClientAsync(string clientId);
        Task<Client> FindClientAsync(string clientId);
        Task<Client> FindClientByNameAsync(string clientName);
    }
}
