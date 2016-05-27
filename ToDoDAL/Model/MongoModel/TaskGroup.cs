using System.Collections.Generic;
using ToDoDAL.Concrete;

namespace ToDoDAL.Model.MongoModel
{
    public class TaskGroup:Entity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public IEnumerable<string> TaskIds { get; set; } 
        public string UserId { get; set; }
    }
}
