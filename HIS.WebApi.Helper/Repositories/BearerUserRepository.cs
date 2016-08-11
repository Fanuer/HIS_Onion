using System;
using System.Data.Entity;
using HIS.WebApi.Auth.Base.Interfaces.Repository;
using HIS.WebApi.Auth.Base.Models;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class BearerUserRepository<TUser>
    : IBearerTokenUserManagementRepository<TUser>,
      IDisposable
      where TUser : class, Interfaces.IUser<string>
  {
    #region FIELDS
    private readonly BearerDbContext _ctx;
    private static IBearerTokenUserManagementRepository<TUser> _instance;
    #endregion

    #region CTOR



    private BearerUserRepository(string bearerDbContextNameOrConnectionString = "AuthContext")
    {
      _ctx = new BearerDbContext(bearerDbContextNameOrConnectionString);
      RefreshTokens = new RefreshTokenDbRepository(_ctx);
      Clients = new ClientDbRepository(_ctx);
    }

    internal BearerUserRepository(BearerDbContext ctx)
    {
      _ctx = ctx;
      RefreshTokens = new RefreshTokenDbRepository(_ctx);
      Clients = new ClientDbRepository(_ctx);
    }

    public BearerUserRepository(Interfaces.Repository.IUserRoleStore<TUser> userRep, string bearerDbContextNameOrConnectionString="AuthContext") 
      : this(bearerDbContextNameOrConnectionString)
    {
      if (userRep == null) { throw new ArgumentNullException(nameof(userRep)); }

      this.Users = userRep;
    }

    public BearerUserRepository(Interfaces.Repository.IUserRoleStore<TUser> userRep, IRefreshTokenRepository refreshTokenRep, IClientRepository clientRep)
    {
      if (userRep == null){throw new ArgumentNullException(nameof(userRep));}
      if (refreshTokenRep == null) { throw new ArgumentNullException(nameof(refreshTokenRep)); }
      if (clientRep == null) { throw new ArgumentNullException(nameof(clientRep)); }

      this.Users = userRep;
      this.RefreshTokens = refreshTokenRep;
      this.Clients = clientRep;
    }

    #endregion

    #region METHODS

    public void Dispose()
    {
      _ctx.Dispose();
    }

    #endregion

    #region PROPERTIES
    public IClientRepository Clients { get; private set; }
    public IRefreshTokenRepository RefreshTokens { get; private set; }

    public Interfaces.Repository.IUserRoleStore<TUser> Users { get; private set; }


    public static IBearerTokenUserManagementRepository<TUser> Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(IBearerTokenUserManagementRepository<TUser>))
          {
            if (_instance == null)
            {
              _instance = new BearerUserRepository<TUser>();
            }
          }
        }
        return _instance;
      }
    }
    #endregion

    #region Nested

    internal class RefreshTokenDbRepository : GenericDbRepository<RefreshToken, string>, IRefreshTokenRepository
    {
      #region Ctor
      public RefreshTokenDbRepository(BearerDbContext ctx) : base(ctx) { }
      #endregion
    }

    internal class ClientDbRepository : GenericDbRepository<Client, string>, IClientRepository
    {
      #region Ctor
      public ClientDbRepository(BearerDbContext ctx) : base(ctx) { }
      #endregion
    }

    #endregion

  }
}
