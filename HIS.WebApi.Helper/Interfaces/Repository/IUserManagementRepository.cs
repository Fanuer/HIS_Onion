using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IUserManagementRepository<TUser, TRole, in TUserKey, in TRoleKey> 
    : IDisposable
      where TUser : class, IUser<TUserKey>
      where TRole : class, IRole<TRoleKey>
  {
    IUserStore<TUser, TUserKey> Users { get; }

    IRoleStore<TRole, TRoleKey> Roles { get; }
  }

  public interface IUserManagementRepository<TUser, TRole>
      where TUser : class, IUser<string>
      where TRole : class, IRole<string>
  {
    IUserStore<TUser> Users { get; }

    IRoleStore<TRole> Roles { get; }
  }

}
