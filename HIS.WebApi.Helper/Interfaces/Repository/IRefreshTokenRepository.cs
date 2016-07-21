using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Interfaces.SingleId;
using HIS.WebApi.Helper.Models;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IRefreshTokenRepository : IRepositoryAddAndDelete<RefreshToken, string>, IRepositoryFindAll<RefreshToken>, IRepositoryFindSingle<RefreshToken, string>
  {
  }
}
