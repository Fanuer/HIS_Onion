using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IUserManagementRepository<TUser, in TUserKey> 
    : IDisposable
      where TUser : class, IUser<TUserKey>
  {
    IUserRoleStore<TUser, TUserKey> Users { get; }
  }

  public interface IUserManagementRepository<TUser>
      where TUser : class, IUser<string>
  {
    IUserRoleStore<TUser> Users { get; }
  }

}
