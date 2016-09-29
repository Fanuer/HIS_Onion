using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Data.Interfaces.MultiId
{
    public interface IRepositoryFindSingleMultiId<T, in TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty[] id);
    }
}