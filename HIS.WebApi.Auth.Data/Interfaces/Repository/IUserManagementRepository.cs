using System;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  public interface IUserManagementRepository<TUser, in TUserKey>
    : IDisposable
      where TUser : class, IUser<TUserKey>
  {
    IUserRoleStore<TUser, TUserKey> Users { get; }
  }

  public interface IUserManagementRepository<TUser>: IDisposable
      where TUser : class, IUser<string>
  {
    IUserRoleStore<TUser> Users { get; }
  }

}
