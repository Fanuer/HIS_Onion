using System;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IApplicationRepository : IDisposable
  {
    IClientRepository Clients { get; }
    IRefreshTokenRepository RefreshTokens { get; }

  }
}
