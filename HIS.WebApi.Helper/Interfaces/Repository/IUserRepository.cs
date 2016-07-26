using HIS.WebApi.Auth.Base.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IUserRepository<TKey> 
    : IRepositoryFindSingle<IUser<TKey>, TKey>, IRepositoryAddAndDelete<IUser<TKey>, TKey>, IRepositoryFindAll<IUser<TKey>>, IRepositoryUpdate<IUser<TKey>, TKey>
  {

  }
}
