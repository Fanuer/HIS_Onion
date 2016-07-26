using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Base.Interfaces
{
    public interface IRepositoryFindAll<T>
    {
        Task<IQueryable<T>> GetAllAsync();
    }
}