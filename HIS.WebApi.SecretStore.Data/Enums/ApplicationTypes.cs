namespace HIS.WebApi.SecretStore.Data.Enums
{
    /// <summary>
    /// Request Type of a Client
    /// </summary>
    public enum ApplicationTypes
    {
        /// <summary>
        /// Client only needs JWT
        /// </summary>
        JavaScript = 0,
        /// <summary>
        /// Client should send the secret once the access token is requested
        /// </summary>
        NativeConfidential = 1
    }
}