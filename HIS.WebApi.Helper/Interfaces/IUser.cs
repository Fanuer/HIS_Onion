using System.Collections.Generic;
using System.Security.Claims;

namespace HIS.WebApi.Auth.Base.Interfaces
{
  public interface IUser<TKey>:Microsoft.AspNet.Identity.IUser<TKey>, IEntity<TKey>
  {
    IEnumerable<Claim> Claims { get; }
    string DisplayName { get; }
  }
}
