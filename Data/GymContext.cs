using Microsoft.EntityFrameworkCore;
using GymManagement.Models;

namespace GymManagement.Data
{
    /// <summary>
    /// Represents the database context for the Gym Management System.
    /// </summary>
    public class GymContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GymContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public GymContext(DbContextOptions<GymContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for Members.
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for MembershipTypes.
        /// </summary>
        public DbSet<MembershipType> MembershipTypes { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Trainers.
        /// </summary>
        public DbSet<Trainer> Trainers { get; set; }
    }
}
