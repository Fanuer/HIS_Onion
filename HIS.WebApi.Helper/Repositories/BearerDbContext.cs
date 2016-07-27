using System;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Base.Repositories
{
  internal class BearerDbContext: DbContext
  {
    #region Fields
    #endregion

    #region Ctor
    public BearerDbContext(string nameOrConnectionString = "AuthContext")
      : base(nameOrConnectionString)
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
