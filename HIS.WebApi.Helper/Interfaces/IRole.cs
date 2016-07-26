namespace HIS.WebApi.Auth.Base.Interfaces
{
  public interface IRole<TKey> : Microsoft.AspNet.Identity.IRole<TKey>, IEntity<TKey>
  {
  }
}
