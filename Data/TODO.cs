using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToDoList.Areas.Identity.Data;

namespace ToDoList.Data
{
    public class TODO
    {
        public long Id { get; set; }
        public User Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Term { get; set; }
        public string Priority { get; set; }
        public bool IsDone { get; set; }
    }
}
