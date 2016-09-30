using System;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.SingleId;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    public interface IClientService : IRepositoryAddAndDelete<Client, string>, IReadOnlyClientService
    {
        Task<Client> AddAsync(string clientName);
    }
}
