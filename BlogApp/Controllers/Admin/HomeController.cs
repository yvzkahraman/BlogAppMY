using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers.Admin
{
    [Area("Admin")]

    //SRP => SINGLE RESPONSIBILITY PRINCIPLE
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
