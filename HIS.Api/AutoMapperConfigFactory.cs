using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using AutoMapper;
using HIS.WebApi.Auth.Base.Interfaces;
using HIS.WebApi.Auth.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Onion.Client;

namespace HIS.WebApi.Auth
{
  public class AutoMapperConfigFactory
  {
    #region FIELDS

    private static AutoMapperConfigFactory _instance;
    #endregion

    #region CTOR

    private AutoMapperConfigFactory()
    {
      
    }
    #endregion

    #region METHODS

    public MapperConfiguration Create()
    {
      return new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<Onion.Client.IUser, IUser<int>>().ConstructUsing(this.FromOnionUser);

      });
    }

    private User FromOnionUser(Onion.Client.IUser user)
    {
      var claims = new Collection<IdentityUserClaim>
      {
        new IdentityUserClaim() {ClaimType = UserRoles.Administrator.ToString(), ClaimValue = user.IsAdministrator.ToString(), UserId = user.Id.ToString()},
        new IdentityUserClaim() {ClaimType = UserRoles.Editor.ToString(), ClaimValue = user.IsEditor.ToString(), UserId = user.Id.ToString()},
        new IdentityUserClaim() {ClaimType = UserRoles.LiveEditor.ToString(), ClaimValue = user.IsLiveEditor.ToString(), UserId = user.Id.ToString()},
        new IdentityUserClaim() {ClaimType = UserRoles.SchemaManager.ToString(), ClaimValue = user.IsSchemaManager.ToString(), UserId = user.Id.ToString()},
        new IdentityUserClaim() {ClaimType = UserRoles.UserManager.ToString(), ClaimValue = user.IsUserManager.ToString(), UserId = user.Id.ToString()},
      };

      return new User(user.Id, user.Name, user.DisplayName, claims);
    }
    #endregion

    #region PROPERTIES

    public static AutoMapperConfigFactory Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(AutoMapperConfigFactory))
          {
            if (_instance == null)
            {
              _instance = new AutoMapperConfigFactory();
            }
          }
        }
        return _instance;
        
      }
    }
    #endregion

  }
}