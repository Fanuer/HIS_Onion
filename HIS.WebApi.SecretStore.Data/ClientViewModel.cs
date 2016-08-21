using System.ComponentModel.DataAnnotations;

namespace HIS.WebApi.SecretStore.Data
{
    /// <summary>
    /// Stores Data of a Client
    /// </summary>
    public class ClientViewModel
    {
        /// <summary>
        /// Client name
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
