using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Areas.Identity.Data;

namespace ToDoList.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string FilePath { get; set; }

        [ForeignKey("CategoryInfoKey")]
        public Category Category { get; set; }
        public User Author { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
