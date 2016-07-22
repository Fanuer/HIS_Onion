using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces
{
  public interface IRole<TKey> : Microsoft.AspNet.Identity.IRole<TKey>, IEntity<TKey>
  {
  }
}
