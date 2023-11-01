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


            if (model.CreatedDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate.Date == model.CreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {

                query = query.Where(x => x.Title.Contains(model.Title));
            }


            // Bir sayfada kaç tane gözüksün => 5: 
            // Hangi sayafadayım ?? 

            //1.sayfada isem => 1 2 3 4 5  => 5            1 2 3 4 5 6 7 8 9 10 .... 50
            //2. sayfada isem=>6 7 8 9 10 => 5

            //...............        . 48   

            // 0 kayıt geç, 5 kayıt çek => 1. sayfa => ActivePage 
            //5 kayıt geç, 5 kayıt çek => 2. sayfa



            ViewBag.PageModel = new PageModel
            {
                ActivePage = model.ActivePage,

                PageCount = (int)Math.Ceiling((decimal)(query.Count()) / model.PageSize),

                PageSize = model.PageSize,
            };



            var blogs = query.Skip((model.ActivePage - 1) * model.PageSize).Take(model.PageSize).ToList();

            /* 5 kayıtı nasıl geçiyor
             Acaba ilk 5 kayıdı hiç okumadan 2.sayfa için  5 kayıt mı seçiyor
             ilk 5 kaydıda okuyor mu ?


            1000 adet kaydımız var 
            1 sayfada 50 adet gösteriyoruz.
            300. kaydım, 50 tane kayıdımı getirir ? 300 tane kayıdımı getirir 1000 tane kaydımı getirir.

            6. sayfanın.


            
            */




            return View(blogs);
        }

    }
}
