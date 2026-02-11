using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models;

namespace GymManagement.Controllers
{
    /// <summary>
    /// Controller for managing membership types.
    /// </summary>
    public class MembershipTypesController : Controller
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipTypesController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MembershipTypesController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all membership types.
        /// </summary>
        /// <returns>The Index view with the list of membership types.</returns>
        // GET: MembershipTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MembershipTypes.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific membership type.
        /// </summary>
        /// <param name="id">The identifier of the membership type.</param>
        /// <returns>The Details view for the specified membership type.</returns>
        // GET: MembershipTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membershipType == null)
            {
                return NotFound();
            }

            return View(membershipType);
        }

        /// <summary>
        /// Displays the Create MembershipType view.
        /// </summary>
        /// <returns>The Create view.</returns>
        // GET: MembershipTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new membership type.
        /// </summary>
        /// <param name="membershipType">The membership type object to create.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: MembershipTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Cost,DurationInMonths")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipType);
        }

        /// <summary>
        /// Displays the Edit MembershipType view.
        /// </summary>
        /// <param name="id">The identifier of the membership type to edit.</param>
        /// <returns>The Edit view for the specified membership type.</returns>
        // GET: MembershipTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }
            return View(membershipType);
        }

        /// <summary>
        /// Updates an existing membership type.
        /// </summary>
        /// <param name="id">The identifier of the membership type to update.</param>
        /// <param name="membershipType">The updated membership type object.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: MembershipTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Cost,DurationInMonths")] MembershipType membershipType)
        {
            if (id != membershipType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipTypeExists(membershipType.Id))
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
            return View(membershipType);
        }

        /// <summary>
        /// Displays the Delete MembershipType view.
        /// </summary>
        /// <param name="id">The identifier of the membership type to delete.</param>
        /// <returns>The Delete view for the specified membership type.</returns>
        // GET: MembershipTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membershipType == null)
            {
                return NotFound();
            }

            return View(membershipType);
        }

        /// <summary>
        /// Confirms the deletion of a membership type.
        /// </summary>
        /// <param name="id">The identifier of the membership type to delete.</param>
        /// <returns>Redirects to Index on success.</returns>
        // POST: MembershipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.Id == id);
        }
    }
}
