using HIS.WebApi.Auth.Data.Interfaces.Models;
using Microsoft.AspNetCore.Identity;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  public interface IUserRoleStore<TUser>: 
        IUserStore<TUser>, 
        Microsoft.AspNetCore.Identity.IUserRoleStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserClaimStore<TUser>
        where TUser : class, IUser<string>
  {
  }
}
