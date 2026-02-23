using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for API registration requests.
    /// </summary>
    public class ApiRegisterDto
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
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password confirmation. Must match Password.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
