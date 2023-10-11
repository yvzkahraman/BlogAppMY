using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data
{

    [Index(nameof(SeoUrl), IsUnique = true)]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string ShortDescription { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(300)]
        public string SeoUrl { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<BlogCategory>? BlogCategories { get; set; }

    }
}
