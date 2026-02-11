namespace GymManagement.Models
{
    /// <summary>
    /// Represents a gym trainer.
    /// </summary>
    public class Trainer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the trainer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the trainer's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the trainer's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the trainer's specialty (e.g., Cardio, Weights, Yoga).
        /// </summary>
        public string Specialty { get; set; }
    }
}
