using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    public interface IBearerTokenUserService<TUser, TUserKey>
      : IUserService<TUser, TUserKey>
        where TUser : class, IUser<TUserKey>
    {
        IClientService Clients { get; }
        IRefreshTokenService RefreshTokens { get; }
    }

    public interface IBearerTokenUserService<TUser>
      : IUserService<TUser>
        where TUser : class, IUser<string>

    {
        IClientService Clients { get; }
        IRefreshTokenService RefreshTokens { get; }
    }
}
