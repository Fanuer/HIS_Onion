using System.ComponentModel.DataAnnotations;

namespace HIS.WebApi.Auth.Data.Models
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
