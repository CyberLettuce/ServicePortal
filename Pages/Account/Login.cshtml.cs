using System.ComponentModel.DataAnnotations;
using ServicePortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your email address or username.")]
        public string Login { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public bool RememberMe { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(user =>
                    user.NormalizedEmail == Login.Trim().ToUpper() ||
                    user.NormalizedUserName == Login.Trim().ToUpper() ||
                    user.DisplayName == Login.Trim());

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email address, username, or password.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                Password,
                RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(ReturnUrl) &&
                    Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return LocalRedirect(Url.Content("~/"));
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account has been temporarily locked because of too many failed sign-in attempts.");

                return Page();
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This account is not currently allowed to sign in.");

                return Page();
            }

            ModelState.AddModelError(
                string.Empty,
                "Invalid email address, username, or password.");

            return Page();
        }
    }
}
