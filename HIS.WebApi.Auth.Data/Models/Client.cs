using System;
using System.ComponentModel.DataAnnotations;
using HIS.Helpers.Crypto;
using HIS.WebApi.Auth.Data.Interfaces.Models;
using HIS.WebApi.Auth.Data.Models.Enums;

namespace HIS.WebApi.Auth.Data.Models
{
    /// <summary>
    /// Defines Client, that uses the AuthServer
    /// </summary>
    public class Client:IEntity<string>
    {
        #region Ctor

        /// <summary>
        /// Creates a new Client-Object
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="secret">Secret, that is used to sign a bearer token</param>
        /// <param name="name">Name of the Client</param>
        /// <param name="allowedOrigin">Origins that are allowed for this client</param>
        /// <param name="active">Is the client active or closed</param>
        /// <param name="type">Request Type of a Client</param>
        /// <param name="timeSpan">Time Span in Month after a Refresh Token shall become invalid</param>
        internal Client(string id = "", string name = "", string secret = "", string allowedOrigin = "*", ApplicationType type = Enums.ApplicationType.JavaScript, int timeSpan = 6, bool active = true)
        {
            this.Id = id;
            this.Secret = secret;
            this.Name = name;
            this.AllowedOrigin = allowedOrigin;
            this.Active = active;
            this.RefreshTokenLifeTime = timeSpan;
            this.ApplicationType = type;
        }

        /// <summary>
        /// Creates a new Client-Object
        /// </summary>
        internal Client() : this("", "", "")
        {

        }

        /// <summary>
        /// Creates a new active Client-Object
        /// </summary>
        /// <param name="name">Name of the Client</param>
        /// <param name="allowedOrigin">Origins that are allowed for this client</param>
        /// <param name="type">Request Type of a Client</param>
        /// <param name="timeSpan">Time Span in Month after a Refresh Token shall become invalid</param>
        public Client(string name, string allowedOrigin = "*")
            :this("", name, allowedOrigin: allowedOrigin)
        {
            if (String.IsNullOrEmpty(name)){ throw new ArgumentNullException(nameof(name)); }

            this.Id = Guid.NewGuid().ToString("N");
            this.Secret = Hasher.Current.CreateHash(this.Id);
        }
        /// <summary>
        /// Creates a new active Client-Object
        /// </summary>
        /// <param name="name">Name of the Client</param>
        public Client(string name):this(name, "*"){}
        #endregion

        #region Method
        /// <summary>
        /// Creates a new Client-Object with the current Values
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Client(this.Id, this.Name, this.Secret, this.AllowedOrigin, this.ApplicationType, this.RefreshTokenLifeTime, this.Active);
        }

        /// <summary>
        /// Checks if the given object has equal properties and has the same type
        /// </summary>
        /// <param name="other">Object to check the equality with</param>
        /// <returns></returns>
        protected bool Equals(Client other)
        {
            return 
                   string.Equals(Id, other.Id) 
                && string.Equals(Secret, other.Secret) 
                && string.Equals(Name, other.Name)
                && string.Equals(AllowedOrigin, other.AllowedOrigin) 
                && Active == other.Active 
                && RefreshTokenLifeTime == other.RefreshTokenLifeTime 
                && ApplicationType == other.ApplicationType;
        }

        /// <summary>
        /// Checks if the given object has equal properties and has the same type
        /// </summary>
        /// <param name="obj">Object to check the equality with</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Client) obj);
        }

        /// <summary>
        /// Creates a hash value for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Secret?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Active.GetHashCode();
                hashCode = (hashCode*397) ^ (AllowedOrigin?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ RefreshTokenLifeTime;
                hashCode = (hashCode*397) ^ (int) ApplicationType;
                return hashCode;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Client Id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Secret, that is used to sign a bearer token
        /// </summary>
        [Required]
        public string Secret { get; set; }

        /// <summary>
        /// Name of the Client
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Is the client active or closed
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Origins that are allowed for this client
        /// </summary>
        [Required]
        public string AllowedOrigin { get; set; }

        /// <summary>
        /// Time Span in Month after a Refresh Token shall become invalid
        /// </summary>
        [Required]
        public int RefreshTokenLifeTime { get; set; }

        /// <summary>
        /// Request Type of a Client
        /// </summary>
        [Required]
        public ApplicationType ApplicationType { get; set; }
        #endregion
    }
}
