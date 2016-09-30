namespace HIS.WebApi.Auth.Data.Interfaces.Models
{
  public interface IUser<TKey>:Microsoft.AspNet.Identity.IUser<TKey>, IEntity<TKey>
  {
    string DisplayName { get; }

    #region Method
    #endregion
  }
}
