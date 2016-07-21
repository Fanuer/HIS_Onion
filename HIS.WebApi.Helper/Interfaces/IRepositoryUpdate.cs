using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces
{
    public interface IRepositoryUpdate<T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}