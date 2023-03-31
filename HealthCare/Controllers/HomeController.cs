using HealthCare.Data;
using HealthCare.Models;
using HealthCare.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Diagnostics;
using System.Security.Claims;

namespace HealthCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<FoodsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<FoodsController> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HealthDiary()
        {
            ViewData["foods"] = _context.Food.ToList();
            ViewData["exercises"] = _context.PhysicalActivity.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] MealRequest request)
        {
            string userId = GetCurrentUserId();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    SaveMeal(request, _context, userId);
                    SaveActivities(request, _context, userId);
                    var existAthleteInformation = _context.AthleteInformation
                        .FirstOrDefault(x => x.AthleteId == userId && x.CreateTime.Date == DateTime.Now.Date);
                    if (existAthleteInformation != null)
                    {
                        existAthleteInformation.AthleteId = userId;
                        existAthleteInformation.WaterIntake = existAthleteInformation.WaterIntake + request.Water;
                        existAthleteInformation.CreateTime = DateTime.Now;
                        existAthleteInformation.CreateBy = userId;
                        _context.AthleteInformation.Update(existAthleteInformation);
                    }
                    else
                    {
                        var information = new AthleteInformation();
                        information.AthleteId = userId;
                        information.WaterIntake = request.Water;
                        information.CreateTime = DateTime.Now;
                        information.CreateBy = userId;
                        await _context.AthleteInformation.AddAsync(information);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                return Ok();
            }
        }


        private void SaveActivities(MealRequest request, ApplicationDbContext dbContext, string userId)
        {
            var activities = _context.PhysicalActivity.Where(x => request.Activities.Contains(x.Id)).ToList();
            foreach (var activityId in request.Activities)
            {
                var activity = new AthleteActivity
                {
                    Activity = activities.First(x => x.Id == activityId),
                    ActivityId = activityId,
                    AthleteId = userId,
                    CreateTime= DateTime.Now,
                    CreateBy= userId
                };
                dbContext.AthleteActivities.Add(activity);
                dbContext.SaveChanges();
            }
        }
        public async Task<IActionResult> AthleteInformation(string id)
        {
            if (id == null)
            {
                id = GetCurrentUserId();
            }
            var now = DateTime.Now.Date;
            var information = await _context.AthleteInformation
                .FirstOrDefaultAsync(x => x.AthleteId == id && x.CreateTime.Date == now);
            if (information != null)
            {
                string userId = GetCurrentUserId();
                information.FoodIntake = _context.Meal
               .Where(x => x.AthleteId == id && x.CreateTime.Date == now)
               .Include(x => x.MealFoods).ThenInclude(x => x.Food).ToList();
                information.Activities = await _context.AthleteActivities
                    .Where(x => x.AthleteId == id && x.CreateTime.Date == now).Include(x => x.Activity).ToListAsync();
                ViewData["user"] = _context.Users.FirstOrDefault(x => x.Id == userId);
                return View(information);
            }

            return View(nameof(Index));
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }

        private void SaveMeal(MealRequest request, ApplicationDbContext dbContext, string userId)
        {
            foreach (var meal in request.Meal)
            {
                var existMealToday = dbContext.Meal
                    .FirstOrDefault(x => x.AthleteId == userId && x.CreateTime.Date == DateTime.Now.Date && x.Type == meal.Type);
                if (existMealToday != null)
                {
                    SaveFoods(dbContext, existMealToday, meal.Foods.ToList(), userId);
                }
                else
                {
                    var newMeal = new Meal();
                    newMeal.AthleteId = userId;
                    newMeal.Type = meal.Type;
                    newMeal.CreateTime = DateTime.Now;
                    newMeal.CreateBy = userId;
                    dbContext.Add(newMeal);
                    SaveFoods(dbContext, newMeal, meal.Foods.ToList(), userId);
                }
            }
        }

        private void SaveFoods(ApplicationDbContext dbContext, Meal meal, List<int> foods, string userId)
        {
            // Clear the existing meal foods
            meal.MealFoods = new List<MealFood>();
            var foodList = _context.Food.Where(x => foods.Contains(x.Id));
            foreach (var foodId in foods)
            {
                var mealFood = new MealFood
                {
                    Meal = meal,
                    Food = foodList.First(x => x.Id == foodId),
                    CreateTime = DateTime.Now,
                    CreateBy = userId
                };

                dbContext.AddAsync(mealFood);
            }

            dbContext.SaveChanges();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult MyAthletes()
        {
            string userId = GetCurrentUserId();
            var users = _context.Users.Where(x => x.CoachId == userId).ToList();
            return View(users);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = _context.MealFoods.FirstOrDefault(x => x.Id == id);

            if (food != null)
            {
                _context.MealFoods.Remove(food);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost, ActionName("DeleteActivity")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = _context.AthleteActivities.FirstOrDefault(x => x.Id == id);

            if (activity != null)
            {
                _context.AthleteActivities.Remove(activity);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}