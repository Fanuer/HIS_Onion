using HIS.WebApi.Helper.Interfaces.Repository;
using HIS.WebApi.Helper.Models;
using HIS.WebApi.Helper.Repositories;

namespace HIS.Api.Repositories
{
  public class Repository : IApplicationRepository<int>
  {
    #region FIELDS
    private readonly AuthContext _ctx;
    private static IApplicationRepository _instance;
    #endregion

    #region CTOR

    private Repository()
    {
      _ctx = ContextFactory.Instance.CreateContext();
      RefreshTokens = new RefreshTokenDbRepository(_ctx);
      Clients = new ClientDbRepository(_ctx);
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

    internal class RefreshTokenDbRepository : GenericDBRepository<RefreshToken, string>, IRefreshTokenRepository
    {
      #region Ctor
      public RefreshTokenDbRepository(AuthContext ctx) : base(ctx) { }
      #endregion
    }

    internal class ClientDbRepository : GenericDBRepository<Client, string>, IClientRepository
    {
      #region Ctor
      public ClientDbRepository(AuthContext ctx) : base(ctx) { }
      #endregion
    }

    #endregion
  }
}
