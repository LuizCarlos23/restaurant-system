using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class Account : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;

        public Account(UserManager<ApplicationUser> userMange, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userMange;
            _signInManager = signInManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel userViewModel, string? returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
                return View(userViewModel);

            var user = new ApplicationUser() { UserName = userViewModel.Email, Email = userViewModel.Email };
            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userViewModel);
            }

            await _signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userViewModel, string? returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
                return View(userViewModel);

            var result = await _signInManager.PasswordSignInAsync(userViewModel.Email, userViewModel.Password, userViewModel.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email ou senha inválido!");
                return View(userViewModel);
            }

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
