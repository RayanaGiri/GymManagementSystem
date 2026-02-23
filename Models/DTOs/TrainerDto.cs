namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for Trainer data exposed via the API.
    /// </summary>
    public class TrainerDto
    {
        /// <summary>
        /// Gets or sets the trainer's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the trainer's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the trainer's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the trainer's specialty.
        /// </summary>
        public string Specialty { get; set; } = string.Empty;
    }
}
