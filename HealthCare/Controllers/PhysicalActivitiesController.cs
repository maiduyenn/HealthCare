using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data;
using HealthCare.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace HealthCare.Controllers
{
    public class PhysicalActivitiesController : Controller
    {
        private readonly ILogger<PhysicalActivitiesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PhysicalActivitiesController(ILogger<PhysicalActivitiesController> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: PhysicalActivities
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhysicalActivity.ToListAsync());
        }

        // GET: PhysicalActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhysicalActivity == null)
            {
                return NotFound();
            }

            var physicalActivity = await _context.PhysicalActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalActivity == null)
            {
                return NotFound();
            }

            return View(physicalActivity);
        }

        // GET: PhysicalActivities/Create
        public IActionResult Create()
        {
            return View();
        }
        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }
        // POST: PhysicalActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Duration,BurnedCalories")] PhysicalActivity physicalActivity)
        {
            var user = GetCurrentUserId();
            physicalActivity.CreateTime = DateTime.Now;
            physicalActivity.CreateBy = user;
            _context.Add(physicalActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PhysicalActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhysicalActivity == null)
            {
                return NotFound();
            }

            var physicalActivity = await _context.PhysicalActivity.FindAsync(id);
            if (physicalActivity == null)
            {
                return NotFound();
            }
            return View(physicalActivity);
        }

        // POST: PhysicalActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Duration,BurnedCalories")] PhysicalActivity physicalActivity)
        {
            if (id != physicalActivity.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(physicalActivity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhysicalActivityExists(physicalActivity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(physicalActivity);
        }

        // GET: PhysicalActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhysicalActivity == null)
            {
                return NotFound();
            }

            var physicalActivity = await _context.PhysicalActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalActivity == null)
            {
                return NotFound();
            }

            return View(physicalActivity);
        }

        // POST: PhysicalActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhysicalActivity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PhysicalActivity'  is null.");
            }
            var physicalActivity = await _context.PhysicalActivity.FindAsync(id);
            if (physicalActivity != null)
            {
                _context.PhysicalActivity.Remove(physicalActivity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhysicalActivityExists(int id)
        {
            return _context.PhysicalActivity.Any(e => e.Id == id);
        }
    }
}
