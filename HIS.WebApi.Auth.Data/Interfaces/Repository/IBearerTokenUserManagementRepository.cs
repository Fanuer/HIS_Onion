namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    public interface IBearerTokenUserManagementRepository<TUser, TUserKey>
      : IUserManagementRepository<TUser, TUserKey>
        where TUser : class, IUser<TUserKey>
    {
        IClientStore Clients { get; }
        IRefreshTokenRepository RefreshTokens { get; }
    }

    public interface IBearerTokenUserManagementRepository<TUser>
      : IUserManagementRepository<TUser>
        where TUser : class, IUser<string>

    {
        IClientStore Clients { get; }
        IRefreshTokenRepository RefreshTokens { get; }
    }
}
