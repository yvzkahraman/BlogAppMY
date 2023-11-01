namespace BlogApp.Models
{
    public class AssignCategoryListModel
    {
        public int BlogId { get; set; }
        public int Id { get; set; }
        public string Definition { get; set; } = null!;

        public bool Exist { get; set; }
    }
}
