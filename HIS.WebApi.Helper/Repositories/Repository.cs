using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Interfaces.Repository;
using HIS.WebApi.Helper.Models;

namespace HIS.WebApi.Helper.Repositories
{
  public class Repository : IApplicationRepository
  {
    #region FIELDS
    private readonly AuthContext _ctx;
    private static IApplicationRepository _instance;
    #endregion

    #region CTOR

    private Repository()
    {
      _ctx = ContextFactory.Instance.CreateContext();
      RefreshTokens = new RefreshTokenRepository(_ctx);
      Clients = new ClientRepository(_ctx);
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

    public static IApplicationRepository Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(IApplicationRepository))
          {
            if (_instance == null)
            {
              Repository._instance = new Repository();
            }
          }
        }
        return _instance;
      }
    }
    #endregion

    #region Nested

    internal class RefreshTokenRepository : GenericRepository<RefreshToken, string>, IRefreshTokenRepository
    {
      #region Ctor
      public RefreshTokenRepository(AuthContext ctx) : base(ctx) { }
      #endregion
    }

    internal class ClientRepository : GenericRepository<Client, string>, IClientRepository
    {
      #region Ctor
      public ClientRepository(AuthContext ctx) : base(ctx) { }
      #endregion
    }

    #endregion
  }
}
