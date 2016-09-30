using System;
using System.ComponentModel.DataAnnotations;
using HIS.WebApi.Auth.Data.Interfaces;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Models
{
  /// <summary>
  /// Represents a Refreshtoken. 
  /// It is used by an application to grant a new accesstoken by the Authorization Server.
  /// </summary>
  public class RefreshToken:IEntity<string>
  {
    #region Ctor

    public RefreshToken()
    {
      
    }

    public RefreshToken(string id, string subject, string clientId, DateTime issuedUtc, DateTime expiresUtc, string protectedTicket)
    {
      Id = id;
      Subject = subject;
      ClientId = clientId;
      IssuedUtc = issuedUtc;
      ExpiresUtc = expiresUtc;
      ProtectedTicket = protectedTicket;
    }
    
    #endregion

    #region Method
    public object Clone()
    {
      return new RefreshToken(Id, Subject, ClientId, IssuedUtc, ExpiresUtc, ProtectedTicket);
    }

    
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Hashed value of the refresh token id
    /// </summary>
    [Key]
    public string Id { get; set; }
    /// <summary>
    /// Indicates to which user this refresh token belongs
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; }
    /// <summary>
    /// Is used to revoke the refresh token for a certain user on certain 
    /// client and keep the other refresh tokens for the same user obtained 
    /// by different clients available.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ClientId { get; set; }
    /// <summary>
    /// Indicated when the Refreshtoken was requestd
    /// </summary>
    public DateTime IssuedUtc { get; set; }
    /// <summary>
    /// Expire Date
    /// </summary>
    public DateTime ExpiresUtc { get; set; }
    /// <summary>
    /// contains all the claims and ticket properties for this user. 
    /// The Owin middle-ware will use this string to build the new access token.
    /// </summary>
    [Required]
    public string ProtectedTicket { get; set; }

    
    #endregion
  }
}
