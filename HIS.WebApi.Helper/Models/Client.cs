using System.ComponentModel.DataAnnotations;
using HIS.WebApi.Auth.Base.Interfaces;

namespace HIS.WebApi.Auth.Base.Models
{
  public class Client : IEntity<string>
  {
    #region Ctor

    public Client(string id = "", string secret = "", string name = "", string allowedOrigin = "*", bool active = true)
    {
      this.Id = id;
      this.Secret = secret;
      this.Name = name;
      this.AllowedOrigin = allowedOrigin;
      this.Active = active;
    }

    public Client() : this("")
    {

    }
    #endregion

    #region Properties
    [Key]
    [MaxLength(32)]
    public string Id { get; set; }

    [MaxLength(80)]
    [Required]
    public string Secret { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    public bool Active { get; set; }

    [MaxLength(100)]
    public string AllowedOrigin { get; set; }
    #endregion
  }
}
