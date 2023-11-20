using BlogApp.Data;
using Microsoft.AspNetCore.Cors;
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

        // A P I ?

        [EnableCors]
        [HttpGet]
        public JsonResult GetCategories()
        {
            Thread.Sleep(2000);

            // LINQ TO DATA,
            // LINQ TO XML 
            // 
            //var query = from c in context.Categories 

            var categories = this.context.Categories.AsNoTracking().ToList();

            return Json(categories);
        }
    }
}
/* 
 JSON 
XML 
 */