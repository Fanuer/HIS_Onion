using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
using HIS.WebApi.Auth.Data.Models;
using HIS.WebApi.Clients.SecretStore;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Auth.Base.Repositories
{
    public class BearerUserRepository<TUser>
      : IBearerTokenUserService<TUser>,
        IDisposable
        where TUser : class, Data.Interfaces.Models.IUser<string>
    {
        #region FIELDS
        #endregion

        #region CTOR

        public BearerUserRepository(Data.Interfaces.Repository.IUserRoleStore<TUser> userRep, IRefreshTokenService refreshTokenRep, IClientService clientService)
        {
            if (userRep == null) { throw new ArgumentNullException(nameof(userRep)); }
            if (refreshTokenRep == null) { throw new ArgumentNullException(nameof(refreshTokenRep)); }
            if (clientService == null) { throw new ArgumentNullException(nameof(clientService)); }

            this.Users = userRep;
            this.RefreshTokens = refreshTokenRep;
            this.Clients = clientService;
        }

        #endregion

        #region METHODS

        public void Dispose()
        {
        }

        #endregion

        #region PROPERTIES
        public IClientService Clients { get; private set; }
        public IRefreshTokenService RefreshTokens { get; private set; }

        public Data.Interfaces.Repository.IUserRoleStore<TUser> Users { get; private set; }

        #endregion

        #region Nested

        internal class RefreshTokenDbService : GenericDbRepository<RefreshToken, string>, IRefreshTokenService
        {
            #region Ctor
            public RefreshTokenDbService(BearerDbContext ctx) : base(ctx) { }
            #endregion

            /// <summary>
            /// Finds all expired entries and removes them
            /// </summary>
            /// <returns></returns>
            public async Task RemoveExpiredRefreshTokensAsync()
            {
                var expired = this.DBContext.Set<RefreshToken>().Where(x => x.ExpiresUtc <= DateTime.UtcNow);
                foreach (var refreshToken in expired)
                {
                    await this.RemoveAsync(refreshToken);
                }
            }
        }

        #endregion
    }
}
