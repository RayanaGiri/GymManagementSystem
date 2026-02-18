using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models;

namespace GymManagement.Controllers
{
    /// <summary>
    /// Controller for managing trainers.
    /// </summary>
    public class TrainersController : Controller
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TrainersController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all trainers.
        /// </summary>
        /// <returns>The Index view with the list of trainers.</returns>
        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainers.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific trainer.
        /// </summary>
        /// <param name="id">The identifier of the trainer.</param>
        /// <returns>The Details view for the specified trainer.</returns>
        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        /// <summary>
        /// Displays the Create Trainer view.
        /// </summary>
        /// <returns>The Create view.</returns>
        // GET: Trainers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new trainer.
        /// </summary>
        /// <param name="trainer">The trainer object to create.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: Trainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Specialty")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        /// <summary>
        /// Displays the Edit Trainer view.
        /// </summary>
        /// <param name="id">The identifier of the trainer to edit.</param>
        /// <returns>The Edit view for the specified trainer.</returns>
        // GET: Trainers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        /// <summary>
        /// Updates an existing trainer.
        /// </summary>
        /// <param name="id">The identifier of the trainer to update.</param>
        /// <param name="trainer">The updated trainer object.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Specialty")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        /// <summary>
        /// Displays the Delete Trainer view.
        /// </summary>
        /// <param name="id">The identifier of the trainer to delete.</param>
        /// <returns>The Delete view for the specified trainer.</returns>
        // GET: Trainers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        /// <summary>
        /// Confirms the deletion of a trainer.
        /// </summary>
        /// <param name="id">The identifier of the trainer to delete.</param>
        /// <returns>Redirects to Index on success.</returns>
        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }
    }
}
