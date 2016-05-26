using ToDoDAL.Abstract;

namespace ToDoDAL.Model.MongoModel
{
    public class Task:IEntity<Task>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; } 
        public string Comment { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
