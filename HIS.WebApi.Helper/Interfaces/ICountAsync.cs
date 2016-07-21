using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces
{
  public interface ICountAsync<T> where T:class
  {
    Task<int> CountAsync<T>() where T : class;
  }
}
