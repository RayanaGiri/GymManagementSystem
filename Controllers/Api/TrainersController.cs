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
    /// API controller for trainer data access, with role-based authorization.
    /// Admin: full CRUD.
    /// User: read-only (list and details).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TrainersController : ControllerBase
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainersController"/> class.
        /// </summary>
        public TrainersController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all trainers. Available to any authenticated user.
        /// </summary>
        /// <returns>A list of all trainers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAll()
        {
            var trainers = await _context.Trainers
                .Select(t => new TrainerDto
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Specialty = t.Specialty
                })
                .ToListAsync();

            return Ok(trainers);
        }

        /// <summary>
        /// Gets a specific trainer by ID. Available to any authenticated user.
        /// </summary>
        /// <param name="id">The trainer ID.</param>
        /// <returns>The trainer with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerDto>> GetById(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
                return NotFound(new { message = "Trainer not found." });

            return Ok(new TrainerDto
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Specialty = trainer.Specialty
            });
        }

        /// <summary>
        /// Creates a new trainer. Admin only.
        /// </summary>
        /// <param name="dto">The trainer data.</param>
        /// <returns>The created trainer.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<TrainerDto>> Create([FromBody] TrainerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trainer = new Trainer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialty = dto.Specialty
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            dto.Id = trainer.Id;
            return CreatedAtAction(nameof(GetById), new { id = trainer.Id }, dto);
        }

        /// <summary>
        /// Updates an existing trainer. Admin only.
        /// </summary>
        /// <param name="id">The trainer ID.</param>
        /// <param name="dto">The updated trainer data.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(int id, [FromBody] TrainerDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch." });

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound(new { message = "Trainer not found." });

            trainer.FirstName = dto.FirstName;
            trainer.LastName = dto.LastName;
            trainer.Specialty = dto.Specialty;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Trainers.Any(t => t.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a trainer. Admin only.
        /// </summary>
        /// <param name="id">The trainer ID.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound(new { message = "Trainer not found." });

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
