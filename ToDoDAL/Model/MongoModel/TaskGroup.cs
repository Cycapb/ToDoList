using System.Collections.Generic;
using ToDoDAL.Abstract;

namespace ToDoDAL.Model.MongoModel
{
    public class TaskGroup:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Task> Tasks { get; set; } 
        public string UserId { get; set; }
    }
}
