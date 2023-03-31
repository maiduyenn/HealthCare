using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class Food : Audit
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public string Nutrition { get; set; }
        public ICollection<MealFood> MealFoods { get; set; }
    }
}
