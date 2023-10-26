using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers.Admin
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly BlogDbContext context;

        public BlogController(BlogDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Index(BlogSearchModel model)
        {
            //if(model.CreatedDate == DateTime.MinValue && string.IsNullOrWhiteSpace(model.Title))
            //{
            //    return View();
            //}

            var query = this.context.Blogs.AsQueryable();

       
            if (model.CreatedDate != DateTime.MinValue )
            {
              query =  query.Where(x=>x.CreatedDate.Date == model.CreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {

               query = query.Where(x => x.Title.Contains(model.Title));
            }
            

            var blogs = query.ToList();
            return View(blogs);
        }

    }
}
