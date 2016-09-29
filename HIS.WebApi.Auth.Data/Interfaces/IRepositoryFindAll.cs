using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Data.Interfaces
{
    public interface IRepositoryFindAll<T>
    {
        Task<IQueryable<T>> GetAllAsync();
    }
}