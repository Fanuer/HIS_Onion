using System.Data.Entity;
using HIS.WebApi.Helper.Models;

namespace HIS.Api.Repositories
{
  public class AuthContext: DbContext
  {
    #region Fields
    #endregion

    #region Ctor
    public AuthContext(string connectionString)
      : base(connectionString)
    {
      this.Database.Log = Console.WriteLine;
      Configuration.ProxyCreationEnabled = false;
      Configuration.LazyLoadingEnabled = false;
    }

    #endregion
    
    #region Properties
    public DbSet<Client> Audiences { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion
  }
}
