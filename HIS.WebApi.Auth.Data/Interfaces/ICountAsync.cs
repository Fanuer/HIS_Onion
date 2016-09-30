using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Data.Interfaces
{
  public interface ICountAsync
  {
      Task<int> CountAsync();
  }
}
