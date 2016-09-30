using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.SingleId
{
    public interface IRepositoryAddAndDelete<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> AddAsync(T model);
        Task<bool> RemoveAsync(TIdProperty id);
        Task<bool> RemoveAsync(T model);
        Task<bool> ExistsAsync(TIdProperty id);
    }
}