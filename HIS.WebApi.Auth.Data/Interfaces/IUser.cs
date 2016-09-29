using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Data.Interfaces
{
  public interface IUser<TKey>:Microsoft.AspNet.Identity.IUser<TKey>, IEntity<TKey>
  {
    string DisplayName { get; }

    #region Method
    #endregion
  }
}
