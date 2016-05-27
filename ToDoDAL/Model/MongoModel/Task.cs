using ToDoDAL.Concrete;

namespace ToDoDAL.Model.MongoModel
{
    public class Task:Entity
    {
        public string Name { get; set; }
        public string GroupId { get; set; } 
        public string Comment { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
