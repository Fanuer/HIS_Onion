using IAspNetRole = Microsoft.AspNet.Identity.IRole;

namespace HIS.WebApi.Auth.Data.Interfaces.Models
{
  public interface IRole<TKey> : Microsoft.AspNet.Identity.IRole<TKey>, IEntity<TKey>
  {
  }
}
