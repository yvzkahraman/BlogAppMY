using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers.Admin
{


    public enum ViewType
    {
        Create = 0,
        Update = 1,
    }



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
     
        public IActionResult CreateOrUpdate(int id, ViewType type)
        {
            ViewBag.Type = type;

            if (type == ViewType.Update)
            {
                var updatedCategory = this.context.Categories.AsNoTracking().SingleOrDefault(x => x.Id == id);


                return View(updatedCategory);

            }
            else
            {
                return View(new Category());
            }
   
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(Category category)
        {

            if(category.Id == 0)
            {
                category.SeoUrl = ConvertSeoUrl(category.Definition);
                this.context.Categories.Add(category);
                this.context.SaveChanges();
            }
            else
            {
                var updatedEntity = this.context.Categories.SingleOrDefault(x => x.Id == category.Id);

                if (updatedEntity != null)
                {

                    if (updatedEntity.Definition != category.Definition)
                    {
                        updatedEntity.Definition = category.Definition;
                        updatedEntity.SeoUrl = ConvertSeoUrl(category.Definition);
                    }

                    this.context.SaveChanges();
                }
               
            }

            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            return View();
        }


        public IActionResult Remove(int id)
        {          
            var deletedCategory = this.context.Categories.SingleOrDefault(x => x.Id == id);
            this.context.Categories.Remove(deletedCategory);

            this.context.SaveChanges();

            return RedirectToAction("Index");
        }



        private string ConvertSeoUrl(string definition)
        {
            definition = definition.ToLower().Replace(' ', '-');
            //şğüöçı
            definition = definition.Replace('ş', 's');
            definition = definition.Replace('ğ', 'g');
            definition = definition.Replace('ü', 'u');
            definition = definition.Replace('ö', 'o');
            definition = definition.Replace('ç', 'c');
            definition = definition.Replace('ı', 'i');
            return definition;
          
        }


    }
}
