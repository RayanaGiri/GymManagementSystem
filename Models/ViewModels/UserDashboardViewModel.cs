using System;

namespace GymManagement.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public Member? Member { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}
