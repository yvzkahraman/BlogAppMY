using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data
{
    [Index(nameof(SeoUrl), IsUnique = true)]
    public class Category : IEquatable<Category>
    {
        // Data Annotation, Fluent Api | Fluent Validation => 

        [Key]
        public int Id { get; set; }

        // attribute
        [MaxLength(250)]
        [Required]
        public string Definition { get; set; } = null!;

        
        [MaxLength(300)] 
        [Required]      
        public string SeoUrl { get; set; } = null!;

        public List<BlogCategory>? BlogCategories { get; set; }

        public bool Equals(Category? other)
        {
           return this.Definition == other.Definition && this.SeoUrl == other.SeoUrl && this.Id == other.Id;
        }
    }
}
