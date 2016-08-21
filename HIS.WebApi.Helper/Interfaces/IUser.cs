using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIS.WebApi.Auth.Base.Interfaces
{
  public interface IUser<TKey>:Microsoft.AspNet.Identity.IUser<TKey>, IEntity<TKey>
  {
    ICollection<Claim> AdditionalClaims { get; }
    string DisplayName { get; }

    #region Method

    Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IUser<string>> manager, string authenticationType);

    #endregion
  }
}
