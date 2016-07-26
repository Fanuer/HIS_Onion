using HIS.WebApi.Auth.Base.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IRoleRepository<TKey>:
      IRepositoryFindSingle<IRole<TKey>, TKey>, IRepositoryAddAndDelete<IRole<TKey>, TKey>, IRepositoryFindAll<IRole<TKey>>, IRepositoryUpdate<IRole<TKey>, TKey>
  {
  }
}
