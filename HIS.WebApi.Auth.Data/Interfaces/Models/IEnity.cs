namespace HIS.WebApi.Auth.Data.Interfaces.Models
{
  public interface IEntity<out T>
  {
    T Id { get; }
  }
}
