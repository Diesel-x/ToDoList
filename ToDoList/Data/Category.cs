namespace ToDoList.Data
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
