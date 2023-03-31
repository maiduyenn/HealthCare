using HealthCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class EditRoleModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EditRoleModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string SelectedRole { get; set; }

    public ApplicationUser User { get; set; }
    public IList<string> UserRoles { get; set; }
    public List<SelectListItem> Roles { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        User = await _userManager.FindByIdAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        Email = User.Email;

        Roles = _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToList();

        UserRoles = await _userManager.GetRolesAsync(User);
        SelectedRole = UserRoles.FirstOrDefault();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id, string[] selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{id}'.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var rolesToAdd = selectedRoles.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(selectedRoles);
        if (rolesToAdd.Contains("Coach"))
        {
            user.IsCoach = true;
        }
        await _userManager.AddToRolesAsync(user, rolesToAdd);
        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        return RedirectToPage("Userlist");
    }
}
