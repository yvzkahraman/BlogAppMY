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
            //return new TestResult().ExecuteResultAsync;
            //()
            ViewBag.Active = "Blog";
            //if(model.CreatedDate == DateTime.MinValue && string.IsNullOrWhiteSpace(model.Title))
            //{
            //    return View();
            //}

            // - bir sayfada kaç tane gözüksün.
            //  hangi sayfadayız ??
            //  kaç sayfam var.


            // 100 =>  8 tane gözüksün
            // 2 sayfadayım. ilk kaydı => 11
            // 


            // PageSize
            // ActivePage
            // Total


           


            var query = this.context.Blogs.AsQueryable();

       
            if (model.CreatedDate != DateTime.MinValue )
            {
              query =  query.Where(x=>x.CreatedDate.Date == model.CreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {

               query = query.Where(x => x.Title.Contains(model.Title));
            }
            

            // 0 kayıt geç, 5 kayıt çek => 1. sayfa => ActivePage 
            //5 kayıt geç, 5 kayıt çek => 2. sayfa




            var blogs = query.Skip((model.ActivePage-1)*model.PageSize).Take(model.PageSize).ToList();
            return View(blogs);
        }

    }
}
