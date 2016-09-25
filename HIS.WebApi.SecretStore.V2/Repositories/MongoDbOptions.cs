using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.SecretStore.V2.Repositories
{
    /// <summary>
    /// Options to initialise MongoDB
    /// </summary>
    public class MongoDbOptions
    {
        /// <summary>
        /// Mongo Db Connection String for the secret store
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// SecretStore database name
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// SecretStore collection name
        /// </summary>
        public string CollectionName { get; set; }
    }
}
