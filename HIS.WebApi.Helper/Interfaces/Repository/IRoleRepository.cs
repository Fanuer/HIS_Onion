using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Interfaces.SingleId;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IRoleRepository<TKey>:
      IRepositoryFindSingle<IRole<TKey>, TKey>, IRepositoryAddAndDelete<IRole<TKey>, TKey>, IRepositoryFindAll<IRole<TKey>>, IRepositoryUpdate<IRole<TKey>, TKey>
  {
  }
}
