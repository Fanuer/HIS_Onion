using System;
using System.Data.Entity;
using HIS.WebApi.Auth.Base.Interfaces.Repository;
using HIS.WebApi.Auth.Base.Models;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class BearerUserRepository<TUserKey, TRoleKey> : IBearerTokenUserManagementRepository<TUserKey, TRoleKey>
  {
    #region FIELDS
    private readonly BearerDbContext _ctx;
    private static IBearerTokenUserManagementRepository<TUserKey, TRoleKey> _instance;
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
      RefreshTokens = new RefreshTokenDbRepository(_ctx);
      Clients = new ClientDbRepository(_ctx);
    }

    public BearerUserRepository(IUserRepository<TUserKey> userRep, IRoleRepository<TRoleKey> roleRep, string bearerDbContextNameOrConnectionString="AuthContext") 
      : this(bearerDbContextNameOrConnectionString)
    {
      if (userRep == null) { throw new ArgumentNullException(nameof(userRep)); }
      if (roleRep == null) { throw new ArgumentNullException(nameof(roleRep)); }


      this.Users = userRep;
      this.Roles = roleRep;
    }

    public BearerUserRepository(IUserRepository<TUserKey> userRep, IRoleRepository<TRoleKey> roleRep, IRefreshTokenRepository refreshTokenRep, IClientRepository clientRep)
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

    public IUserRepository<TUserKey> Users { get; private set; }
    public IRoleRepository<TRoleKey> Roles { get; private set; }


    public static IBearerTokenUserManagementRepository<TUserKey, TRoleKey> Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(IBearerTokenUserManagementRepository<TUserKey, TRoleKey>))
          {
            if (_instance == null)
            {
              _instance = new BearerUserRepository<TUserKey, TRoleKey>();
            }
          }
        }
        return _instance;
      }
    }
    #endregion

    #region Nested

    internal class RefreshTokenDbRepository : GenericDBRepository<RefreshToken, string>, IRefreshTokenRepository
    {
      #region Ctor
      public RefreshTokenDbRepository(BearerDbContext ctx) : base(ctx) { }
      #endregion
    }

    internal class ClientDbRepository : GenericDBRepository<Client, string>, IClientRepository
    {
      #region Ctor
      public ClientDbRepository(BearerDbContext ctx) : base(ctx) { }
      #endregion
    }

    #endregion

  }
}
