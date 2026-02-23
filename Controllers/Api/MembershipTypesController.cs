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
    /// API controller for membership type data access, with role-based authorization.
    /// Admin: full CRUD.
    /// User: read-only (list and details).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MembershipTypesController : ControllerBase
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipTypesController"/> class.
        /// </summary>
        public MembershipTypesController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all membership types. Available to any authenticated user.
        /// </summary>
        /// <returns>A list of all membership types.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetAll()
        {
            var types = await _context.MembershipTypes
                .Select(mt => new MembershipTypeDto
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    Cost = mt.Cost,
                    DurationInMonths = mt.DurationInMonths
                })
                .ToListAsync();

            return Ok(types);
        }

        /// <summary>
        /// Gets a specific membership type by ID. Available to any authenticated user.
        /// </summary>
        /// <param name="id">The membership type ID.</param>
        /// <returns>The membership type with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipTypeDto>> GetById(int id)
        {
            var mt = await _context.MembershipTypes.FindAsync(id);

            if (mt == null)
                return NotFound(new { message = "Membership type not found." });

            return Ok(new MembershipTypeDto
            {
                Id = mt.Id,
                Name = mt.Name,
                Cost = mt.Cost,
                DurationInMonths = mt.DurationInMonths
            });
        }

        /// <summary>
        /// Creates a new membership type. Admin only.
        /// </summary>
        /// <param name="dto">The membership type data.</param>
        /// <returns>The created membership type.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<MembershipTypeDto>> Create([FromBody] MembershipTypeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var membershipType = new MembershipType
            {
                Name = dto.Name,
                Cost = dto.Cost,
                DurationInMonths = dto.DurationInMonths
            };

            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();

            dto.Id = membershipType.Id;
            return CreatedAtAction(nameof(GetById), new { id = membershipType.Id }, dto);
        }

        /// <summary>
        /// Updates an existing membership type. Admin only.
        /// </summary>
        /// <param name="id">The membership type ID.</param>
        /// <param name="dto">The updated membership type data.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(int id, [FromBody] MembershipTypeDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch." });

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
                return NotFound(new { message = "Membership type not found." });

            membershipType.Name = dto.Name;
            membershipType.Cost = dto.Cost;
            membershipType.DurationInMonths = dto.DurationInMonths;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MembershipTypes.Any(mt => mt.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a membership type. Admin only.
        /// </summary>
        /// <param name="id">The membership type ID.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
                return NotFound(new { message = "Membership type not found." });

            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
