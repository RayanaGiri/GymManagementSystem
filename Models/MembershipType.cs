using System.Collections.Generic;

namespace GymManagement.Models
{
    /// <summary>
    /// Represents a type of membership available at the gym.
    /// </summary>
    public class MembershipType
    {
        /// <summary>
        /// Gets or sets the unique identifier for the membership type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the membership type (e.g., Gold, Silver).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cost of the membership.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the duration of the membership in months.
        /// </summary>
        public int DurationInMonths { get; set; }

        /// <summary>
        /// Gets or sets the collection of members who have this membership type.
        /// </summary>
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}