using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    /// <summary>
    /// Represents a membership plan.
    /// </summary>
    public class MembershipType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int DurationInMonths { get; set; }

        // Collection of members with this membership type
        public ICollection<Member> Members { get; set; }
    }
}
