using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class MealFood : Audit
    {
        [Key]
        public int Id { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }

    }
}
