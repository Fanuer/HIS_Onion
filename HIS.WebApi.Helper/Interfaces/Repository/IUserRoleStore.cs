using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IUserRoleStore<TUser, in TKey>: IUserStore<TUser, TKey>, Microsoft.AspNet.Identity.IUserRoleStore<TUser, TKey> where TUser : class, IUser<TKey>
  {
  }

  public interface IUserRoleStore<TUser> : HIS.WebApi.Auth.Base.Interfaces.Repository.IUserRoleStore<TUser, string> where TUser : class, IUser<string>
  {
  }
}
