using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class Meal:Audit
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
         public ICollection<MealFood> MealFoods { get; set; }
        public string AthleteId { get; set; }
        public ApplicationUser Athlete { get; set; }
    }
}
