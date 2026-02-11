using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    /// <summary>
    /// Represents a gym member.
    /// </summary>
    public class Member
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime JoinDate { get; set; }
        
        // Foreign key for MembershipType
        public int MembershipTypeId { get; set; }

        // Navigation property for MembershipType
        public MembershipType MembershipType { get; set; }
    }
}
