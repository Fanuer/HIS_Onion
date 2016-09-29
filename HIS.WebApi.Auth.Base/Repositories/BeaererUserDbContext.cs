using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Base.Repositories
{
  internal class BeaererUserDbContext: IdentityDbContext<User>
  {
    #region Fields
    #endregion

    #region Ctor
    public BeaererUserDbContext(string nameOrConnectionString = "AuthContext")
      : base(nameOrConnectionString)
    {
      this.Database.Log = Console.WriteLine;
      Configuration.ProxyCreationEnabled = false;
      Configuration.LazyLoadingEnabled = false;
    }

    #endregion

    #region Properties
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion

  }
}
