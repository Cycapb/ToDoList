using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers
{
    [RoutePrefix("api/todo")]
    public class ToDoController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityValueProvider<ToDoList> _valueProvider;

        public ToDoController(IEntityValueProvider<ToDoList> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        [Route("{id:int}")]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> GetToDoList(int id)
        {
            var item = (await _valueProvider.GetValuesAsync())
                .Where(x => x.NoteId == id)
                .Select(x => new TodoListDto()
                {
                   NoteId = id,
                   Comment = x.Comment,
                   GroupId = x.GroupId,
                   GroupName = x.Group.Name,
                   Name = x.Name,
                   StatusId = x.StatusId,
                   UserId = x.UserId
                })
                .SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{userId}")]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IQueryable<TodoListDto>> GetTodoByUser(string userId)
        {
            return  (await _valueProvider.GetValuesAsync())
                .Where(x => x.UserId == userId)
                .Select(x => new TodoListDto
                {
                    NoteId = x.NoteId,
                    Comment = x.Comment,
                    GroupId = x.GroupId,
                    GroupName = x.Group.Name,
                    Name = x.Name,
                    StatusId = x.StatusId,
                    UserId = x.UserId
                });
        }

        [HttpPost]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> CreateToDoList(ToDoList item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(item);
                return CreatedAtRoute("DefaultApi", new {id = item.NoteId}, item);
            }
            Logger.Warn($"User tried to create null ToDo item data from ip: {HttpContext.Current.Request.UserHostAddress}");
            return BadRequest(ModelState);
        }

        [HttpPut]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> UpdateToDo(int id, ToDoList item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return StatusCode(HttpStatusCode.NoContent);
            }
            Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
            return BadRequest(ModelState);
        }

        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> DeleteToDoList(int id)
        {
            var item = await _valueProvider.GetValueAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _valueProvider.DeleteValueAsync(id);
            return Ok(item);
        }

        [HttpPut]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> UpdateToDoList([FromBody]IEnumerable<ToDoList> items)
        {
            if (items == null)
            {
                Logger.Warn($"User tried to update data from ip: {HttpContext.Current.Request.UserHostAddress}");
                return BadRequest();
            }
            await _valueProvider.UpdateValuesAsync(items);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}