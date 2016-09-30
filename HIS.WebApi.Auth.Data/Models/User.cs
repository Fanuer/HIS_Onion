using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Interfaces.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Data.Models
{
  public class User: IdentityUser, Interfaces.Models.IUser<int>, Interfaces.Models.IUser<string>
  {
    #region FIELDS

    private string _displayName;
    #endregion

    #region CTOR

    private User()
    {
    }

    public User(int id):this()
    {
      this.Id = id;
    }

    public User(int id, string userName, string displayName=""):this(id)
    {
      if (String.IsNullOrWhiteSpace(userName)){throw new ArgumentNullException(nameof(userName));}

      UserName = userName;
      this.DisplayName = displayName;
    }
    #endregion

    #region METHODS
    #endregion

    #region PROPERTIES
    public int Id { get; set; }

    string Microsoft.AspNet.Identity.IUser<string>.Id => this.Id.ToString();

    string IEntity<string>.Id => this.Id.ToString();

    public override string UserName { get; set; }

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

  

    #endregion
  }
}