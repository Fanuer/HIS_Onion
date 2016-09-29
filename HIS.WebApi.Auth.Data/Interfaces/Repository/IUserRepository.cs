using System;
using HIS.WebApi.Auth.Data.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
  /// <summary>
  /// Definies the DBAccess for Users
  /// </summary>
  /// <typeparam name="TUser">Type of User-Entity</typeparam>
  /// <typeparam name="TKey">Type of Id-Property of the User</typeparam>
  public interface IUserRepository<TUser, TKey>
    : IRepositoryFindSingle<TUser, TKey>, IRepositoryAddAndDelete<TUser, TKey>, IRepositoryFindAll<TUser>, IRepositoryUpdate<TUser, TKey>, IDisposable where TUser:class, IUser<TKey>
  {

  }

  /// <summary>
  /// Definies the DBAccess for Users
  /// </summary>
  /// <typeparam name="TUser">Type of User-Entity with a string Id</typeparam>
  public interface IUserRepository<TUser>
    : IRepositoryFindSingle<TUser, string>, IRepositoryAddAndDelete<TUser, string>, IRepositoryFindAll<TUser>, IRepositoryUpdate<TUser, string>, IDisposable where TUser : class, IUser<string>
  {

  }
}
