using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Base.Options
{
    public class ClientOption
    {
        /// <summary>
        /// Client Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client Secret
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// Client Name
        /// </summary>
        public string Issuer { get; set; }
    }
}
