using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Base.Models
{
  internal class User: IdentityUser, HIS.WebApi.Auth.Base.Interfaces.IUser<string>
  {

    #region FIELDS
    #endregion

    #region CTOR
    #endregion

    #region METHODS
    
    #endregion

    #region PROPERTIES
    public string DisplayName { get; set; }
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Interfaces.IUser<string>> manager, string authenticationType)
    {
      var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

      return userIdentity;
    }

    #endregion

  }
}
