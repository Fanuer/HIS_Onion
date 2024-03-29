﻿using System;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Auth.Base.Repositories
{
  public class BearerDbContext: DbContext
  {
    #region Fields
    #endregion

    #region Ctor

    internal BearerDbContext() : this("AuthContext"){}

    public BearerDbContext(string nameOrConnectionString = "AuthContext")
      : base(nameOrConnectionString)
    {
      this.Database.Log = Console.WriteLine;
      Configuration.ProxyCreationEnabled = false;
      Configuration.LazyLoadingEnabled = false;
    }
    
    #endregion
    
    #region Properties
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion
  }
}
