using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    /// <summary>
    /// Represents a gym trainer.
    /// </summary>
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Specialty { get; set; }
    }
}
