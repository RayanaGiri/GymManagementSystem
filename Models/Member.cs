using System;

namespace GymManagement.Models
{
    /// <summary>
    /// Represents a gym member.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Gets or sets the unique identifier for the member.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the member's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the member's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the member's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the member's phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the date the member joined.
        /// </summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the membership type associated with the member.
        /// </summary>
        public int MembershipTypeId { get; set; }

        /// <summary>
        /// Gets or sets the membership type associated with the member.
        /// </summary>
        public MembershipType MembershipType { get; set; }
    }
}
