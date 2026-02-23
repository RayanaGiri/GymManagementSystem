namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for MembershipType data exposed via the API.
    /// </summary>
    public class MembershipTypeDto
    {
        /// <summary>
        /// Gets or sets the membership type's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the membership type.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cost of the membership.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the duration of the membership in months.
        /// </summary>
        public int DurationInMonths { get; set; }
    }
}
