using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Data.Interfaces.SingleId
{
    public interface IRepositoryFindSingle<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty id);
}
}