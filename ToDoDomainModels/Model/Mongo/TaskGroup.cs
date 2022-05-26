using System.Collections.Generic;

namespace ToDoDomainModels.Model.Mongo
{
    public class TaskGroup:Entity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public IEnumerable<string> TaskIds { get; set; } 
        public string UserId { get; set; }
    }
}
