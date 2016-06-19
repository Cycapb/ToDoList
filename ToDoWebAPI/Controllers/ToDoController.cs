using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model;

namespace ToDoWebAPI.Controllers
{
    public class ToDoController : ApiController
    {
        private readonly IEntityValueProvider<ToDoList> _valueProvider;

        public ToDoController(IEntityValueProvider<ToDoList> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public IEnumerable<ToDoList> GetToDoList()
        {
            return _valueProvider.GetValues().ToList();
        }

        public ToDoList GetToDoList(int id)
        {
            return _valueProvider.GetValue(id);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage CreateToDoList(ToDoList item)
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

        [System.Web.Http.HttpPut]
        public HttpResponseMessage UpdateToDoList(ToDoList item)
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

        public HttpResponseMessage DeleteToDoList(int id)
        {
           _valueProvider.DeleteValue(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}