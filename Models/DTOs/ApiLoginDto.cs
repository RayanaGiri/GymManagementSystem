using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for API login requests.
    /// </summary>
    public class ApiLoginDto
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
