using System;
using System.Collections.Generic;
using System.Security.Claims;
using HIS.WebApi.Auth.Base.Interfaces;

namespace HIS.WebApi.Auth.Models
{
  public class User: IUser<int>
  {
    #region FIELDS

    private string _displayName;
    #endregion

    #region CTOR

    private User()
    {
      this.Claims = new List<Claim>();
    }

    public User(int id):this()
    {
      this.Id = id;
    }

    public User(int id, string userName, string displayName="", IEnumerable<Claim> claims= null):this(id)
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

    public IEnumerable<Claim> Claims { get; set; }
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

    #endregion


  }
}