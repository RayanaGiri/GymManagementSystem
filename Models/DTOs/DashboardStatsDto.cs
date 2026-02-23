namespace GymManagement.Models.DTOs
{
    /// <summary>
    /// Data transfer object for dashboard statistics exposed via the API.
    /// Contains admin-level aggregate stats and/or user-level membership info.
    /// </summary>
    public class DashboardStatsDto
    {
        /// <summary>
        /// Gets or sets the total number of members (Admin only).
        /// </summary>
        public int? TotalMembers { get; set; }

        /// <summary>
        /// Gets or sets the total number of trainers (Admin only).
        /// </summary>
        public int? TotalTrainers { get; set; }

        /// <summary>
        /// Gets or sets the total number of membership types (Admin only).
        /// </summary>
        public int? TotalMembershipTypes { get; set; }

        /// <summary>
        /// Gets or sets the current user's member profile (User role).
        /// </summary>
        public MemberDto? MyProfile { get; set; }

        /// <summary>
        /// Gets or sets whether the user's profile is complete (User role).
        /// </summary>
        public bool? IsProfileComplete { get; set; }
    }
}
