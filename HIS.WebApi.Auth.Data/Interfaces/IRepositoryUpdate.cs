using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Data.Interfaces
{
    public interface IRepositoryUpdate<T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}