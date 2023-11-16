using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDbContext context;

        public HomeController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            Thread.Sleep(3000);

            var categories = this.context.Categories.AsNoTracking().ToList();

            return Json(categories);
        }
    }
}
