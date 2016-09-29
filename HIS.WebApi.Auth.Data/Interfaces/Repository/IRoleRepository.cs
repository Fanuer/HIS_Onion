using HIS.WebApi.Auth.Data.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  public interface IRoleRepository<TRole, TKey>:
      IRepositoryFindSingle<TRole, TKey>, IRepositoryAddAndDelete<TRole, TKey>, IRepositoryFindAll<IRole<TKey>>, IRepositoryUpdate<TRole, TKey> where TRole : class, IRole<TKey>
  {
  }

  public interface IRoleRepository<TRole> :
      IRepositoryFindSingle<TRole, string>, IRepositoryAddAndDelete<TRole, string>, IRepositoryFindAll<TRole>, IRepositoryUpdate<TRole, string> where TRole:class, IEntity<string>
  {
  }
}
