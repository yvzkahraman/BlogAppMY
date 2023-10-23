using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers.Admin
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly BlogDbContext context;

        public CategoryController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var categories = this.context.Categories.AsNoTracking().ToList();
            return View(categories);
        }
    }
}
