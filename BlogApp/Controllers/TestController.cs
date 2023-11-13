using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class TestController : Controller
    {
        private readonly BlogDbContext context;

        public TestController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            //var categories =  this.context.Categories.AsQueryable();

            // categories = categories.Where(x => x.Definition == "Alt Giyim");

            // var list = categories.ToList();



            // var categories2 = this.context.Categories.AsEnumerable();

            // categories2 = categories2.Where(x => x.Definition == "Alt Giyim");

            // var list2 = categories2.ToList();

            var updatedUser = new AppUser()
            {
                Id=4,
                Name="Ali",
                Password="1",
                Surname="AliS",
                Username = "ali"
            };

            this.context.Update<AppUser>(updatedUser);
            this.context.SaveChanges();

            return View();
        }


    }
}
