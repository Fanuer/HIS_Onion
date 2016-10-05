using System;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    /// <summary>
    /// Provides access to user data
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    public interface IUserService<TUser>
      : IDisposable
        where TUser : class, IUser<string>
    {
        IUserRoleStore<TUser> Users { get; }
    }
}
