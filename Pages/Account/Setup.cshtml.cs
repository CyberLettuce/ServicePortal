using System.ComponentModel.DataAnnotations;
using ServicePortal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Pages.Account
{
    [AllowAnonymous]
    public class SetupModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public SetupModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbFactory = dbFactory;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a display name.")]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please confirm the password.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(Password),
            ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var userExists = await _userManager.Users.AnyAsync();

            if (userExists)
            {
                return LocalRedirect(Url.Content("~/Account/Login"));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Only allow setup while there are no users.
            var userExists = await _userManager.Users.AnyAsync();

            if (userExists)
            {
                return LocalRedirect(Url.Content("~/Account/Login"));
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            const string administratorRole = "Administrator";

            // Make sure the Administrator role exists.
            if (!await _roleManager.RoleExistsAsync(administratorRole))
            {
                var createRoleResult = await _roleManager.CreateAsync(
                    new IdentityRole(administratorRole));

                if (!createRoleResult.Succeeded)
                {
                    foreach (var error in createRoleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description);
                    }

                    return Page();
                }
            }

            // Create the first user.
            var user = new ApplicationUser
            {
                UserName = Email,
                Email = Email,
                DisplayName = DisplayName,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(
                user,
                Password);

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return Page();
            }

            // Assign Administrator role.
            var assignRoleResult = await _userManager.AddToRoleAsync(
                user,
                administratorRole);

            if (!assignRoleResult.Succeeded)
            {
                foreach (var error in assignRoleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await _userManager.DeleteAsync(user);

                return Page();
            }

            await using (var db = await _dbFactory.CreateDbContextAsync())
            {
                db.UserModules.AddRange(
                    new UserModule { UserId = user.Id, ModuleKey = ModuleKeys.Tickets },
                    new UserModule { UserId = user.Id, ModuleKey = ModuleKeys.Onboarding },
                    new UserModule { UserId = user.Id, ModuleKey = ModuleKeys.Testing },
                    new UserModule { UserId = user.Id, ModuleKey = ModuleKeys.Projects });
                await db.SaveChangesAsync();
            }

            // Sign the new administrator in immediately.
            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            return LocalRedirect(Url.Content("~/"));
        }
    }
}
