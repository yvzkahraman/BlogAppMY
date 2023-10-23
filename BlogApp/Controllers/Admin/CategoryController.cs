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

        public IActionResult Update(int id)
        {
            var updatedCategory = this.context.Categories.AsNoTracking().SingleOrDefault(x => x.Id == id);


            return View(updatedCategory);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {

            category.SeoUrl = category.Definition;

            var updatedEntity =  this.context.Categories.SingleOrDefault(x=>x.Id == category.Id);   
            
            if(updatedEntity != null) {

                updatedEntity.Definition = category.Definition;
                updatedEntity.SeoUrl = category.SeoUrl;

                this.context.SaveChanges();
            }
          
            
            //Disconnected entity
            //this.context.Update(category);
            //this.context.SaveChanges();


            return RedirectToAction("Index"); 

        }
    }
}
