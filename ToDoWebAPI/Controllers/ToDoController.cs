using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoDAL.Model.MongoModel;
using ToDoWebAPI.Abstract;


namespace ToDoWebAPI.Controllers
{
    public class ToDoController : ApiController
    {
        private readonly IMongoValueProvider<Task> _valueProvider;

        public ToDoController(IMongoValueProvider<Task> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public IEnumerable<Task> GetToDoList()
        {
            return _valueProvider.GetValues().ToList();
        }

        public Task GetToDoList(string id)
        {
            return _valueProvider.GetValue(id);
        }

        [HttpPost]
        public HttpResponseMessage CreateToDoList(Task item)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.CreateValue(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateToDoList(Task item)
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

        public HttpResponseMessage DeleteToDoList(string id)
        {
           _valueProvider.DeleteValue(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}