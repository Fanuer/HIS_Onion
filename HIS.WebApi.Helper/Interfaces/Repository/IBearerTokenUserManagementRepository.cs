using System;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IBearerTokenUserManagementRepository<TUserKey, TRoleKey> : IUserManagementRepository<TUserKey, TRoleKey>
  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }
  }
}
