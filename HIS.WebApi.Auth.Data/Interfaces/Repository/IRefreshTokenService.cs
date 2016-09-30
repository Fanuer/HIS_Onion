using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.SingleId;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    public interface IRefreshTokenService : IRepositoryAddAndDelete<RefreshToken, string>, IRepositoryFindSingle<RefreshToken, string>
    {
        /// <summary>
        /// Removes all Refresntokens, that are expired
        /// </summary>
        /// <returns></returns>
        Task RemoveExpiredRefreshTokensAsync();
    }
}
