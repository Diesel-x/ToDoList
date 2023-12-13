using ToDoList.Areas.Identity.Data;

namespace ToDoList.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Text { get; set; }
    }
}
