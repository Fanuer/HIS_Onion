using System;
using System.Data.Entity;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class AuthContext: DbContext
  {
    #region Fields
    #endregion

    #region Ctor
    public AuthContext()
      : base("AuthContext")
    {
      this.Database.Log = Console.WriteLine;
      Configuration.ProxyCreationEnabled = false;
      Configuration.LazyLoadingEnabled = false;
    }
    
    #endregion
    
    #region Properties
    public DbSet<Client> Clients { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion
  }
}
