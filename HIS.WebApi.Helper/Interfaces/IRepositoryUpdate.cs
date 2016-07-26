using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Base.Interfaces
{
    public interface IRepositoryUpdate<T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}