using HIS.WebApi.Auth.Data.Interfaces.Models;
using HIS.WebApi.Auth.Data.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  public interface IRoleService<TRole, TKey>:
      IRepositoryFindSingle<TRole, TKey>, IRepositoryAddAndDelete<TRole, TKey>, IRepositoryFindAll<IRole<TKey>>, IRepositoryUpdate<TRole, TKey> where TRole : class, IRole<TKey>
  {
  }

  public interface IRoleService<TRole> :
      IRepositoryFindSingle<TRole, string>, IRepositoryAddAndDelete<TRole, string>, IRepositoryFindAll<TRole>, IRepositoryUpdate<TRole, string> where TRole:class, IEntity<string>
  {
  }
}
