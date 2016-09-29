using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  public interface IUserRoleStore<TUser, in TKey>: IUserStore<TUser, TKey>, Microsoft.AspNet.Identity.IUserRoleStore<TUser, TKey> where TUser : class, IUser<TKey>
  {
  }

  public interface IUserRoleStore<TUser> : IUserRoleStore<TUser, string> where TUser : class, IUser<string>
  {
  }
}
