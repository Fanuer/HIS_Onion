using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.SingleId;
using HIS.WebApi.Auth.Data.Models;

namespace HIS.WebApi.Auth.Data.Interfaces.Repository
{
    public interface IReadOnlyClientService: IDisposable, IRepositoryFindSingle<Client, string>
    {
    }
}
