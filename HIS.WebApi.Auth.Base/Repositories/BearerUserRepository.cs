using System;
using System.Data.Entity;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
using HIS.WebApi.Auth.Data.Models;
using HIS.WebApi.Clients.SecretStore;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class BearerUserRepository<TUser>
    : IBearerTokenUserManagementRepository<TUser>,
      IDisposable
      where TUser : class, Data.Interfaces.IUser<string>
  {
    #region FIELDS
    #endregion

    #region CTOR

    public BearerUserRepository(Data.Interfaces.Repository.IUserRoleStore<TUser> userRep, IRefreshTokenRepository refreshTokenRep, IClientStore clientStore)
    {
      if (userRep == null){throw new ArgumentNullException(nameof(userRep));}
      if (refreshTokenRep == null) { throw new ArgumentNullException(nameof(refreshTokenRep)); }
      if (clientStore == null) { throw new ArgumentNullException(nameof(clientStore)); }

      this.Users = userRep;
      this.RefreshTokens = refreshTokenRep;
      this.Clients = clientStore;
    }

    #endregion

    #region METHODS

    public void Dispose()
    {
    }

    #endregion

    #region PROPERTIES
    public IClientStore Clients { get; private set; }
    public IRefreshTokenRepository RefreshTokens { get; private set; }

    public Data.Interfaces.Repository.IUserRoleStore<TUser> Users { get; private set; }

    #endregion

    #region Nested

    internal class RefreshTokenDbRepository : GenericDbRepository<RefreshToken, string>, IRefreshTokenRepository
    {
      #region Ctor
      public RefreshTokenDbRepository(BearerDbContext ctx) : base(ctx) { }
      #endregion
    }

    #endregion
  }
}
