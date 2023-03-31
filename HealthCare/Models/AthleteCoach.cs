using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class AthleteCoach : Audit
    {
        [Key]
        public int Id { get; set; }
        public string CoachId { get; set; }
        public ApplicationUser Coach { get; set; }
        public string AthleteId { get; set; }
        public ApplicationUser Athlete { get; set; }

    }
}
