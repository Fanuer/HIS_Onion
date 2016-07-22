using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces
{
  public interface IUser<TKey>:Microsoft.AspNet.Identity.IUser<TKey>, IEntity<TKey>
  {
    IEnumerable<Claim> Claims { get; }
    string DisplayName { get; }
  }
}
