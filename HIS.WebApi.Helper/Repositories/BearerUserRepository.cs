using System;
using System.Data.Entity;
using HIS.WebApi.Auth.Base.Interfaces;
using HIS.WebApi.Auth.Base.Interfaces.Repository;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class BearerUserRepository<TUser, TRole>
    : IBearerTokenUserManagementRepository<TUser, TRole>
      where TUser : class, IUser<string>
      where TRole : class, IRole<string>
  {
    #region FIELDS
    private readonly BearerDbContext _ctx;
    private static IBearerTokenUserManagementRepository<TUser, TRole> _instance;
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

    public BearerUserRepository(IUserRepository<TUser> userRep, IRoleRepository<TRole> roleRep, string bearerDbContextNameOrConnectionString="AuthContext") 
      : this(bearerDbContextNameOrConnectionString)
    {
      if (userRep == null) { throw new ArgumentNullException(nameof(userRep)); }
      if (roleRep == null) { throw new ArgumentNullException(nameof(roleRep)); }


      this.Users = userRep;
      this.Roles = roleRep;
    }

    public BearerUserRepository(IUserRepository<TUser> userRep, IRoleRepository<TRole> roleRep, IRefreshTokenRepository refreshTokenRep, IClientRepository clientRep)
    {
      if (userRep == null){throw new ArgumentNullException(nameof(userRep));}
      if (roleRep == null) { throw new ArgumentNullException(nameof(roleRep)); }
      if (refreshTokenRep == null) { throw new ArgumentNullException(nameof(refreshTokenRep)); }
      if (clientRep == null) { throw new ArgumentNullException(nameof(clientRep)); }

      this.Users = userRep;
      this.Roles = roleRep;
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

    public IUserRepository<TUser> Users { get; private set; }
    public IRoleRepository<TRole> Roles { get; private set; }


    public static IBearerTokenUserManagementRepository<TUser, TRole> Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(IBearerTokenUserManagementRepository<TUser, TRole>))
          {
            if (_instance == null)
            {
              _instance = new BearerUserRepository<TUser, TRole>();
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
