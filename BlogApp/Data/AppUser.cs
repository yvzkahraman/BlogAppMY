using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Surname { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Password { get; set; } = null!;
    }
}
