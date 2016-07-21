using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Helper.Interfaces.SingleId;
using HIS.WebApi.Helper.Models;

namespace HIS.WebApi.Helper.Interfaces.Repository
{
  public interface IClientRepository : IRepositoryFindSingle<Client, string>, IRepositoryAddAndDelete<Client, string>, IRepositoryFindAll<Client>
  {

  }
}
