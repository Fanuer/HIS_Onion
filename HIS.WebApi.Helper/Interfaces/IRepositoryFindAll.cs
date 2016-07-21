using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces
{
    public interface IRepositoryFindAll<T>
    {
        Task<IQueryable<T>> GetAllAsync();
    }
}