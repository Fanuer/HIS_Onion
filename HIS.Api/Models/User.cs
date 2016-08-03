using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Base.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Models
{
  public class User: IdentityUser<int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>, Base.Interfaces.IUser<int>
  {
    #region FIELDS

    private string _displayName;
    #endregion

    #region CTOR

    private User()
    {
      this.Claims = new Collection<IdentityUserClaim>();
    }

    public User(int id):this()
    {
      this.Id = id;
    }

    public User(int id, string userName, string displayName="", Collection<IdentityUserClaim> claims = null):this(id)
    {
      if (String.IsNullOrWhiteSpace(userName)){throw new ArgumentNullException(nameof(userName));}
      UserName = userName;
      this.DisplayName = displayName;

      if (claims != null)
      {
        Claims = claims;
      }
    }
    #endregion

    #region METHODS
    #endregion

    #region PROPERTIES
    public int Id { get; private set; }

    public string UserName { get; set; }

    public ICollection<IdentityUserClaim> Claims { get; set; }

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

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Base.Interfaces.IUser<string>> manager, string authenticationType)
    {
      throw new NotImplementedException();
    }

    #endregion


  }
}