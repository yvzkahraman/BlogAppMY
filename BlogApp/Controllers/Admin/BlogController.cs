using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Validators;
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
            var validator = new BlogCreateModelValidator();
            var validationResult = validator.Validate(model);

            if (validationResult.IsValid)
            {
                var addedBlog = new Blog();

                var extension = Path.GetExtension(model.Image.FileName);
                var newImageName = Guid.NewGuid().ToString() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newImageName);
                var fileStream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(fileStream);
                addedBlog.ImageUrl = newImageName;

                addedBlog.CreatedDate = DateTime.Now;
                addedBlog.Description = model.Description;
                addedBlog.ShortDescription = model.ShortDescription;
                addedBlog.SeoUrl = ConvertSeoUrl(model.Title);
                addedBlog.Title = model.Title;

                this.context.Add(addedBlog);
                this.context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

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
            var updatedBlog = this.context.Blogs.SingleOrDefault(x => x.Id == id);
            return View(new BlogUpdateModel
            {
                Id = updatedBlog.Id,
                ImageUrl = updatedBlog.ImageUrl,
                Description = updatedBlog.Description,
                ShortDescription = updatedBlog.ShortDescription,
                Title = updatedBlog.Title,
            });
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
        public IActionResult Update(BlogUpdateModel model)
        {

            var validator = new BlogUpdateModelValidator();
            var validationResult = validator.Validate(model);

            if (validationResult.IsValid)
            {
                var updatedBlog = this.context.Blogs.SingleOrDefault(x => x.Id == model.Id);

                if (updatedBlog != null)
                {
                    if (!string.IsNullOrWhiteSpace(updatedBlog.ImageUrl) && model.Image != null)
                    {
                        var pathControl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", updatedBlog.ImageUrl);

                        FileInfo fileInfo = new FileInfo(pathControl);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }


                        var extension = Path.GetExtension(model.Image.FileName);
                        var newImageName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newImageName);
                        var fileStream = new FileStream(path, FileMode.Create);
                        model.Image.CopyTo(fileStream);

                        updatedBlog.ImageUrl = newImageName;
                    }


                    updatedBlog.SeoUrl = ConvertSeoUrl(model.Title);
                    updatedBlog.ShortDescription = model.ShortDescription;
                    updatedBlog.Title = model.Title;
                    updatedBlog.Description = model.Description;
                    this.context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Böyle bir blog şuanda mevcut değil";
                    return View(model);
                }
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }
            
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
