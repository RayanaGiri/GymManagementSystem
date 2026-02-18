using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models;

using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Controllers
{
    /// <summary>
    /// Controller for managing members.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MembersController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all members.
        /// </summary>
        /// <returns>The Index view with the list of members.</returns>
        // GET: Members
        public async Task<IActionResult> Index()
        {
            var gymContext = _context.Members.Include(m => m.MembershipType);
            return View(await gymContext.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific member.
        /// </summary>
        /// <param name="id">The identifier of the member.</param>
        /// <returns>The Details view for the specified member.</returns>
        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        /// <summary>
        /// Displays the Create Member view.
        /// </summary>
        /// <returns>The Create view.</returns>
        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["MembershipTypeId"] = new SelectList(_context.MembershipTypes, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Creates a new member.
        /// </summary>
        /// <param name="member">The member object to create.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,JoinDate,MembershipTypeId")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MembershipTypeId"] = new SelectList(_context.MembershipTypes, "Id", "Name", member.MembershipTypeId);
            return View(member);
        }

        /// <summary>
        /// Displays the Edit Member view.
        /// </summary>
        /// <param name="id">The identifier of the member to edit.</param>
        /// <returns>The Edit view for the specified member.</returns>
        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["MembershipTypeId"] = new SelectList(_context.MembershipTypes, "Id", "Name", member.MembershipTypeId);
            return View(member);
        }

        /// <summary>
        /// Updates an existing member.
        /// </summary>
        /// <param name="id">The identifier of the member to update.</param>
        /// <param name="member">The updated member object.</param>
        /// <returns>Redirects to Index on success, or redisplays the form on failure.</returns>
        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,JoinDate,MembershipTypeId")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            ViewData["MembershipTypeId"] = new SelectList(_context.MembershipTypes, "Id", "Name", member.MembershipTypeId);
            return View(member);
        }

        /// <summary>
        /// Displays the Delete Member view.
        /// </summary>
        /// <param name="id">The identifier of the member to delete.</param>
        /// <returns>The Delete view for the specified member.</returns>
        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        /// <summary>
        /// Confirms the deletion of a member.
        /// </summary>
        /// <param name="id">The identifier of the member to delete.</param>
        /// <returns>Redirects to Index on success.</returns>
        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
