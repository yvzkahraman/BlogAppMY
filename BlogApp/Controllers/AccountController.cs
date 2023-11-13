using BlogApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogApp.Data;
using Microsoft.EntityFrameworkCore;
using BlogApp.Validators;

namespace BlogApp.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AccountController : Controller
    {
        private readonly BlogDbContext context;

        public AccountController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Login()
        {
            return View(new UserLoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var validator = new UserLoginModelValidator();
            var validationResult = validator.Validate(model);

            if (validationResult.IsValid)
            {
                var user = this.context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
                if (user != null)
                {
                    await SetAuthCokiee(user, model.RememberMe);
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    return View(model);
                }
            
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }





        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
       CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }


             
        private async Task SetAuthCokiee(AppUser user, bool rememberMe)
        {
            var claims = new List<Claim>
        {

            new Claim("Firstname", user.Name),
            new Claim("Surname", user.Surname),
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = rememberMe,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
