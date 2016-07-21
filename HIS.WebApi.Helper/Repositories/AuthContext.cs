using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Models;

namespace HIS.WebApi.Helper.Repositories
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
