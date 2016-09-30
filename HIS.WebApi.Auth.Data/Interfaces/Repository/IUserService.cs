using System;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    /// <summary>
    /// Provides access to user data
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    /// <typeparam name="TUserKey">type of the Id of a unser object</typeparam>
    public interface IUserService<TUser, in TUserKey>
      : IDisposable
        where TUser : class, IUser<TUserKey>
    {
        IUserRoleStore<TUser, TUserKey> Users { get; }
    }

    /// <summary>
    /// Provides access to user data
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    public interface IUserService<TUser> : IDisposable
        where TUser : class, IUser<string>
    {
        IUserRoleStore<TUser> Users { get; }
    }

}
