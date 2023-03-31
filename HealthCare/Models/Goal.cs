using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class Goal
    {
        [Key]
        public int GoalId { get; set; }
        public int WeightGoal { get; set; }
        public int CaloriesGoal { get; set; }
    }
}
