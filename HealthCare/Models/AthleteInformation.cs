using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class AthleteInformation : Audit
    {
        [Key]
        public int AthleteInformationId { get; set; }
        public string AthleteId { get; set; }
        public int WaterIntake { get; set; }
        public int FoodInTakeDay { get; set; }
        public ApplicationUser Athlete { get; set; }
        public ICollection<Meal> FoodIntake { get; set; }
        public List<AthleteActivity> Activities { get; set; }
    }
}
