using IAspNetRole = Microsoft.AspNet.Identity.IRole;

namespace HIS.WebApi.Auth.Data.Interfaces
{
  public interface IRole<TKey> : Microsoft.AspNet.Identity.IRole<TKey>, IEntity<TKey>
  {
  }
}
