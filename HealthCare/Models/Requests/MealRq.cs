namespace HealthCare.Models.Requests
{
    public class MealRq
    {
        public string Type { get; set; }
        public ICollection<int> Foods { get; set; }
    }
}
