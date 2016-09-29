namespace HIS.Helpers.WebApi.Options
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
