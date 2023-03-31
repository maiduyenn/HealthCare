using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class AthleteActivity : Audit
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public PhysicalActivity Activity { get; set; }
        public string AthleteId { get; set; }
        public ApplicationUser Athlete { get; set; }

    }
}
