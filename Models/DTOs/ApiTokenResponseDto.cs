namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object returned on successful authentication, containing the JWT token.
    /// </summary>
    public class ApiTokenResponseDto
    {
        /// <summary>
        /// Gets or sets the JWT bearer token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token expiration date/time (UTC).
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the authenticated user's email.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of roles assigned to the user.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
