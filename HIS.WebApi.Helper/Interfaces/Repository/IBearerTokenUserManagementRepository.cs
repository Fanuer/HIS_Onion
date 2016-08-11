using System;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IBearerTokenUserManagementRepository<TUser, TUserKey>
    : IUserManagementRepository<TUser, TUserKey>
      where TUser : class, IUser<TUserKey>
  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }
  }

  public interface IBearerTokenUserManagementRepository<TUser>
    : IUserManagementRepository<TUser>
      where TUser : class, IUser<string> 

  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }
  }
}
