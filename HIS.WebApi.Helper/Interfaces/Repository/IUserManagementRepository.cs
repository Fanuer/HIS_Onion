using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Base.Interfaces.Repository
{
  public interface IUserManagementRepository<TUserKey, TRoleKey> : IDisposable
  {
    IUserRepository<TUserKey> Users { get; }

    IRoleRepository<TRoleKey> Roles { get; }
  }
}
