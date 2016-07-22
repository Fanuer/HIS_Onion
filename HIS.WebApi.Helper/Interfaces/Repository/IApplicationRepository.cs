using System;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IApplicationRepository<TUserKey, TRoleKey> : IDisposable
  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    IUserRepository<TUserKey> Users { get; }

    IRoleRepository<TRoleKey> Roles { get; }
  }
}
