using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Concrete
{
    public class GroupProvider:IEntityValueProvider<Group>
    {
        public IEnumerable<Group> GetValues()
        {
            throw new NotImplementedException();
        }

        public Group GetValue(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateValue(Group item)
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(Group item)
        {
            throw new NotImplementedException();
        }
    }
}