using ToDoDAL.Abstract;

namespace ToDoDAL.Model.MongoModel
{
    public class TaskGroup:IEntity<TaskGroup>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
