using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
