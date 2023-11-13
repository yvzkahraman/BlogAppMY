using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers.Admin
{
    [Authorize]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly BlogDbContext context;

        public BlogController(BlogDbContext context)
        {
            this.context = context;
        }

        //public IActionResult RedirectIndex()
        //{
        //    return RedirectToAction("Index", new
        //    {
        //        Title = "",
        //        CreatedDate = DateTime.MinValue,
        //    });
        //}

        public IActionResult Create()
        {
            return View(new BlogCreateModel());
        }

        [HttpPost]
        public IActionResult Create(BlogCreateModel model)
        {
            this.context.Add(new Blog
            {
                CreatedDate = DateTime.Now,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                SeoUrl = ConvertSeoUrl(model.Title),
                ImageUrl = model.ImageUrl,
                Title = model.Title,

            });
            this.context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Index(BlogSearchModel model)
        {
            ViewBag.SearchModel = model;
            ViewBag.Active = "Blog";

            var query = this.context.Blogs.AsQueryable();

            if (model.CreatedDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate.Date == model.CreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {

                query = query.Where(x => x.Title.Contains(model.Title));
            }

            ViewBag.PageModel = new PageModel
            {
                ActivePage = model.ActivePage,

                PageCount = (int)Math.Ceiling((decimal)(query.Count()) / model.PageSize),

                PageSize = model.PageSize,
            };



            var blogs = query.Skip((model.ActivePage - 1) * model.PageSize).Take(model.PageSize).ToList();
            return View(blogs);
        }


        [HttpGet]
        public IActionResult AssignCategory(int id)
        {
            ViewBag.SelectedBlog = this.context.Blogs.AsNoTracking().SingleOrDefault(x => x.Id == id);



            var list = new List<AssignCategoryListModel>();

            var allCategories = this.context.Categories.AsNoTracking().ToList();


            var filtredCategories = this.context.Categories.Join(this.context.BlogCategories, category => category.Id, blog => blog.CategoryId, (category, categoryBlog) => new
            {
                category,
                categoryBlog,
            }).Where(x => x.categoryBlog.BlogId == id).Select(x => new Category
            {
                Definition = x.category.Definition,
                Id = x.category.Id,
                SeoUrl = x.category.SeoUrl,
            }).AsNoTracking().ToList();


            foreach (var category in allCategories)
            {
                bool exist = false;
                if (filtredCategories.Contains(category))
                {
                    exist = true;
                }
                list.Add(new AssignCategoryListModel
                {
                    BlogId = id,
                    Definition = category.Definition,
                    Id = category.Id,
                    Exist = exist
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult AssignCategory(List<AssignCategoryListModel> model)
        {
            // exist => true,
            // exist => false

            foreach (var assignCategory in model)
            {
                var control = this.context.BlogCategories.SingleOrDefault(x => x.BlogId == assignCategory.BlogId && x.CategoryId == assignCategory.Id);
                if (assignCategory.Exist)
                {

                    if (control == null)
                    {
                        this.context.BlogCategories.Add(new BlogCategory { CategoryId = assignCategory.Id, BlogId = assignCategory.BlogId });
                        this.context.SaveChanges();
                    }
                }

                else
                {
                    if (control != null)
                    {
                        this.context.BlogCategories.Remove(control);
                        this.context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Update(int id)
        {
            var updatedBlog = this.context.Blogs.SingleOrDefault(x=>x.Id == id);
            return View(updatedBlog);
        }

        public IActionResult Remove(int id)
        {
            var removedBlog = this.context.Blogs.SingleOrDefault(x => x.Id == id);
            if (removedBlog != null)
            {
                this.context.Remove(removedBlog);
                this.context.SaveChanges();
            }
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Blog blog)
        {

            var updatedBlog = this.context.Blogs.SingleOrDefault(x => x.Id == blog.Id);
            if (updatedBlog != null)
            {
                updatedBlog.SeoUrl = ConvertSeoUrl(blog.SeoUrl);
                updatedBlog.ShortDescription = blog.ShortDescription;
                updatedBlog.Title = blog.Title;
                updatedBlog.Description = blog.Description;
                this.context.SaveChanges();

                
            }
            else
            {
                ViewBag.ErrorMessage = "Böyle bir blog şuanda mevcut değil";
                return View(blog);
            }

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
