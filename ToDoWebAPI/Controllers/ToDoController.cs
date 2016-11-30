using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<ToDoList>> GetToDoList()
        {
            return (await _valueProvider.GetValuesAsync()).ToList();
        }

        public async Task<ToDoList> GetToDoList(int id)
        {
            return await _valueProvider.GetValueAsync(id);
        }

        public async Task<IEnumerable<ToDoList>> GetTodoByUser(string userId)
        {
            return (await _valueProvider.GetValuesAsync()).Where(x => x.UserId == userId).ToList();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateToDoList(ToDoList item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                Logger.Warn($"User tried to create null ToDo item data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateToDo(int id, ToDoList item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public async Task<HttpResponseMessage> DeleteToDoList(int id)
        {
            await _valueProvider.DeleteValueAsync(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateToDoList([FromBody]IEnumerable<ToDoList> items)
        {
            if (items == null)
            {
                Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                await _valueProvider.UpdateValuesAsync(items);
                return new HttpResponseMessage(HttpStatusCode.OK);
            } 
        }
    }
}