using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HomeAccountingSystem_DAL.Abstract;
using ToDoDAL.Model.MongoModel;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IMongoValueProvider<TaskGroup> _valueProvider;

        public GroupController(IMongoValueProvider<TaskGroup> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public IEnumerable<TaskGroup> GetValues(IWorkingUser user)
        {
            if (user == null)
            {
                return null;
            }
            else
            {
                return _valueProvider.GetValues();
            }
        }

        public TaskGroup GetGroup(string id)
        {
            return _valueProvider.GetValue(id);
        }

        [HttpPost]
        public HttpResponseMessage CreateGroup(TaskGroup group)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.CreateValue(group);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateGroup(TaskGroup item)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.UpdateValue(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage DeleteGroup(string id)
        {
            _valueProvider.DeleteValue(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
