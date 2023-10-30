namespace BlogApp.Models
{
    public class PageModel
    {
        public int PageSize { get; set; } = 5;
        public int ActivePage { get; set; } = 1;

        public int PageCount { get; set; }
    }
}
