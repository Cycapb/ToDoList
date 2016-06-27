using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NLog;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model;

namespace ToDoWebAPI.Controllers
{
    public class ToDoController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
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

        [HttpPost]
        public HttpResponseMessage CreateToDoList(ToDoList item)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.CreateValue(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                Logger.Warn($"User tried to create null ToDo item data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateToDo(int id, ToDoList item)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.UpdateValue(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage DeleteToDoList(int id)
        {
           _valueProvider.DeleteValue(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage UpdateToDoList([FromBody]IEnumerable<ToDoList> items)
        {
            if (items == null)
            {
                Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                _valueProvider.UpdateValues(items);
                return new HttpResponseMessage(HttpStatusCode.OK);
            } 
        }
    }
}