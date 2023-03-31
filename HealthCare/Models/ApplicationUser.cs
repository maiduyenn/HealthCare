using HealthCare.Data;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Height { get; set; }
        public Gender Gender { get; set; }
        public int CurrentWeight { get; set; }
        public int TargetWeight { get; set; }
        public string Role { get; set; }
        public bool IsCoach { get; set; }
        public string? CoachId { get; set; }
        public ICollection<ApplicationUser> Athletes { get; set; }
    }
}
