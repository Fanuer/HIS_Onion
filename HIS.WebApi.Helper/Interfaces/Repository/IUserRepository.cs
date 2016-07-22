using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Interfaces.SingleId;
using Microsoft.AspNet.Identity;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IUserRepository<TKey> 
    : IRepositoryFindSingle<IUser<TKey>, TKey>, IRepositoryAddAndDelete<IUser<TKey>, TKey>, IRepositoryFindAll<IUser<TKey>>, IRepositoryUpdate<IUser<TKey>, TKey>
  {

  }
}
