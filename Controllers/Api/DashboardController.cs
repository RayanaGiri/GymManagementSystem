using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models.DTOs;

namespace GymManagement.Controllers.Api
{
    /// <summary>
    /// API controller for dashboard statistics, with role-based data access.
    /// Admin: sees aggregate stats (total members, trainers, membership types).
    /// User: sees own membership profile info.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets dashboard statistics based on the authenticated user's role.
        /// Admin users receive aggregate counts; regular users receive their own profile data.
        /// </summary>
        /// <returns>Dashboard statistics tailored to the user's role.</returns>
        [HttpGet]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var isAdmin = User.IsInRole("Admin");

            if (isAdmin)
            {
                var stats = new DashboardStatsDto
                {
                    TotalMembers = await _context.Members.CountAsync(),
                    TotalTrainers = await _context.Trainers.CountAsync(),
                    TotalMembershipTypes = await _context.MembershipTypes.CountAsync()
                };
                return Ok(stats);
            }

            // Regular authenticated user — return own profile info.
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "Unable to determine user identity." });

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Email == userEmail);

            var userStats = new DashboardStatsDto
            {
                IsProfileComplete = member != null,
                MyProfile = member != null ? new MemberDto
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    JoinDate = member.JoinDate,
                    MembershipTypeId = member.MembershipTypeId,
                    MembershipTypeName = member.MembershipType?.Name ?? string.Empty
                } : null
            };

            return Ok(userStats);
        }
    }
}
