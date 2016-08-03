using System;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IBearerTokenUserManagementRepository<TUser, TRole, TUserKey, TRoleKey>
    : IUserManagementRepository<TUser, TRole, TUserKey, TRoleKey>
      where TUser : class, IUser<TUserKey>
      where TRole : class, IRole<TRoleKey>
  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }
  }

  public interface IBearerTokenUserManagementRepository<TUser, TRole>
    : IUserManagementRepository<TUser, TRole>
      where TUser : class, IUser<string> 
      where TRole : class, IRole<string>

  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }
  }
}
