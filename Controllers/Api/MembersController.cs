using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models;
using GymManagement.Models.DTOs;

namespace GymManagement.Controllers.Api
{
    /// <summary>
    /// API controller for member data access, with role-based authorization.
    /// Admin: full CRUD on all members.
    /// User: read own profile only.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MembersController : ControllerBase
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        public MembersController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all members. Admin only.
        /// </summary>
        /// <returns>A list of all members.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            var members = await _context.Members
                .Include(m => m.MembershipType)
                .Select(m => MapToDto(m))
                .ToListAsync();

            return Ok(members);
        }

        /// <summary>
        /// Gets a specific member by ID. Admin only.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>The member with the specified ID.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
                return NotFound(new { message = "Member not found." });

            return Ok(MapToDto(member));
        }

        /// <summary>
        /// Gets the currently authenticated user's own member profile.
        /// Available to any authenticated user.
        /// </summary>
        /// <returns>The authenticated user's member profile, or 404 if not found.</returns>
        [HttpGet("me")]
        public async Task<ActionResult<MemberDto>> GetMyProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "Unable to determine user identity." });

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Email == userEmail);

            if (member == null)
                return NotFound(new { message = "Member profile not found. Please contact an administrator." });

            return Ok(MapToDto(member));
        }

        /// <summary>
        /// Creates a new member. Admin only.
        /// </summary>
        /// <param name="dto">The member data.</param>
        /// <returns>The created member.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<MemberDto>> Create([FromBody] MemberDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = new Member
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                JoinDate = dto.JoinDate,
                MembershipTypeId = dto.MembershipTypeId
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            // Reload with MembershipType included.
            var created = await _context.Members
                .Include(m => m.MembershipType)
                .FirstAsync(m => m.Id == member.Id);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
        }

        /// <summary>
        /// Updates an existing member. Admin only.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <param name="dto">The updated member data.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(int id, [FromBody] MemberDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch." });

            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound(new { message = "Member not found." });

            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;
            member.Email = dto.Email;
            member.PhoneNumber = dto.PhoneNumber;
            member.JoinDate = dto.JoinDate;
            member.MembershipTypeId = dto.MembershipTypeId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Members.Any(m => m.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a member. Admin only.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound(new { message = "Member not found." });

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Maps a Member entity to a MemberDto.
        /// </summary>
        private static MemberDto MapToDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                JoinDate = member.JoinDate,
                MembershipTypeId = member.MembershipTypeId,
                MembershipTypeName = member.MembershipType?.Name ?? string.Empty
            };
        }
    }
}
