namespace HIS.WebApi.Auth.Data.Interfaces.Models
{
    public interface IRole<out TKey> : IEntity<TKey>
    {
        string Name { get; set; }
    }
}
