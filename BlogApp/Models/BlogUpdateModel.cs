namespace BlogApp.Models
{
    public class BlogUpdateModel
    {
        public int Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        public string? Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
    }
}
