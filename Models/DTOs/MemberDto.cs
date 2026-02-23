namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for Member data exposed via the API.
    /// </summary>
    public class MemberDto
    {
        /// <summary>
        /// Gets or sets the member's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the member's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date the member joined.
        /// </summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated membership type.
        /// </summary>
        public int MembershipTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the associated membership type.
        /// </summary>
        public string MembershipTypeName { get; set; } = string.Empty;
    }
}
