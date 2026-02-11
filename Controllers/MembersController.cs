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
    /// Controller for managing Members.
    /// </summary>
    public class MembersController : Controller
    {
        private readonly GymContext _context;

        public MembersController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all members.
        /// </summary>
        // GET: Members
        public async Task<IActionResult> Index()
        {
            var gymContext = _context.Members.Include(m => m.MembershipType);
            return View(await gymContext.ToListAsync());
        }

        /// <summary>
        /// Displays details for a specific member.
        /// </summary>
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
        /// Displays the create member form.
        /// </summary>
        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["MembershipTypeId"] = new SelectList(_context.MembershipTypes, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Handles the creation of a new member.
        /// </summary>
        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,JoinDate,MembershipTypeId")] Member member)
        {
            // Remove navigation property validation error
            ModelState.Remove("MembershipType");

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
        /// Displays the edit member form.
        /// </summary>
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
        /// Handles the updates to an existing member.
        /// </summary>
        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,JoinDate,MembershipTypeId")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

             // Remove navigation property validation error
             ModelState.Remove("MembershipType");

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
        /// Displays the delete confirmation page.
        /// </summary>
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
        /// Handles the deletion of a member.
        /// </summary>
        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
