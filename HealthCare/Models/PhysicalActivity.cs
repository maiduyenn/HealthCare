using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class PhysicalActivity : Audit
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public int Duration { get; set; }
        public string BurnedCalories { get; set; }
        public string? AthleteId { get; set; }
        public ApplicationUser Athlete { get; set; }
    }
}
