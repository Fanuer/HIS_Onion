using HIS.WebApi.Auth.Base.Interfaces.SingleId;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IRefreshTokenRepository : IRepositoryAddAndDelete<RefreshToken, string>, IRepositoryFindAll<RefreshToken>, IRepositoryFindSingle<RefreshToken, string>
  {
  }
}
