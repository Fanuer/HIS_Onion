using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Base.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Base.Models
{
  public class User: IdentityUser, Interfaces.IUser<int>, Base.Interfaces.IUser<string>
  {
    #region FIELDS

    private string _displayName;
    private string _password;
    #endregion

    #region CTOR

    private User()
    {
      this.AdditionalClaims = new Collection<Claim>();
    }

    public User(int id):this()
    {
      this.Id = id;
    }

    public User(int id, string userName, string displayName="", Collection<Claim> claims = null):this(id)
    {
      if (String.IsNullOrWhiteSpace(userName)){throw new ArgumentNullException(nameof(userName));}

      UserName = userName;
      this.DisplayName = displayName;

      if (claims != null)
      {
        AdditionalClaims = claims;
      }
    }
    #endregion

    #region METHODS
    #endregion

    #region PROPERTIES
    public int Id { get; set; }

    string Microsoft.AspNet.Identity.IUser<string>.Id => this.Id.ToString();

    string IEntity<string>.Id => this.Id.ToString();

    public string UserName { get; set; }

    public ICollection<Claim> AdditionalClaims { get; set; }

    public string DisplayName
    {
      get
      {
        return this._displayName ?? this.UserName;
      }
      set
      {
        this._displayName = value;
      }
    }

    public string Password
    {
      internal get { return this.Password; }
      set { this.Password = value; }
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Base.Interfaces.IUser<string>> manager, string authenticationType)
    {
      var claims = await manager.ClaimsIdentityFactory.CreateAsync(manager, this, authenticationType);
      
      return claims;
    }

    #endregion
  }
}