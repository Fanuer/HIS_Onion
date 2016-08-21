using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.SecretStore.Data.Enums;

namespace HIS.WebApi.SecretStore.Data
{
    /// <summary>
    /// Defines Client, that uses the AuthServer
    /// </summary>
    public class Client
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
        public Client(string id = "", string secret = "", string name = "", string allowedOrigin = "*", bool active = true, ApplicationTypes type = ApplicationTypes.NativeConfidential, int timeSpan = 6)
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
        public Client() : this("")
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// Creates a new Client-Object with the current Values
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Client(this.Id, this.Secret, this.Name, this.AllowedOrigin, this.Active, this.ApplicationType, this.RefreshTokenLifeTime);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Client Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Secret, that is used to sign a bearer token
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Name of the Client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the client active or closed
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Origins that are allowed for this client
        /// </summary>
        public string AllowedOrigin { get; set; }

        /// <summary>
        /// Time Span in Month after a Refresh Token shall become invalid
        /// </summary>
        public int RefreshTokenLifeTime { get; set; }

        /// <summary>
        /// Request Type of a Client
        /// </summary>
        public ApplicationTypes ApplicationType { get; set; }
        #endregion
    }
}
