using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data;
using GymManagement.Models;
using GymManagement.Models.ViewModels;

namespace GymManagement.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly GymContext _context;

        public DashboardController(GymContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var model = new AdminDashboardViewModel
            {
                TotalMembers = await _context.Members.CountAsync(),
                TotalTrainers = await _context.Trainers.CountAsync(),
                TotalMembershipTypes = await _context.MembershipTypes.CountAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Member()
        {
            var userEmail = User.Identity?.Name;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.Email == userEmail);

            var model = new UserDashboardViewModel
            {
                Member = member,
                IsProfileComplete = member != null
            };

            return View(model);
        }
    }
}
