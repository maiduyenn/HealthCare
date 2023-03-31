using Newtonsoft.Json;

namespace HealthCare.Models.Requests
{
    public class MealRequest
    {
        public List<int> Activities { get; set; }
        public ICollection<MealRq> Meal { get; set; }
        public int Water { get; set; }
       
    }
}
