using HIS.WebApi.Auth.Base.Interfaces.SingleId;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IClientRepository : IRepositoryFindSingle<Client, string>, IRepositoryAddAndDelete<Client, string>, IRepositoryFindAll<Client>
  {

  }
}
