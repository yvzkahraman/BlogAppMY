using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(UserLoginModel model)
        {
            //var cookie = HttpContext.Response.Cookies.Append("","",new CookieOptions
            //{
                
            //})
            return View();
        }
    }
}
