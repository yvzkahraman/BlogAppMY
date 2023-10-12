using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
