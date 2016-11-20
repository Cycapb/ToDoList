using System.Collections.Generic;

namespace ToDoDAL.Model
{
    public partial class Group
    {
        public Group()
        {
            ToDoList = new List<ToDoList>();
        }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public  string UserId { get; set; }

        public ICollection<ToDoList> ToDoList { get; set; }
    }
}