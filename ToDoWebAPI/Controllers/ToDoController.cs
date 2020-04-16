using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoDAL.Model;
using ToDoProviders;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers
{
    [RoutePrefix("api/todo")]
    public class ToDoController : ApiController
    {
        private readonly IEntityValueProvider<TodoItem> _valueProvider;

        public ToDoController(IEntityValueProvider<TodoItem> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        [Route("{id:int}")]
        [ResponseType(typeof(TodoListDto))]
        public async Task<IHttpActionResult> GetToDoList(int id)
        {
            var item = (await _valueProvider.GetValuesAsync())?
                .Where(x => x.Id == id)
                .Select(x => new TodoListDto()
                {
                    NoteId = x.Id,
                    Description = x.Description,
                    GroupName = x.Group.Name,
                    StatusId = x.IsFinished,
                })
                .SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{userId}")]
        public async Task<IQueryable<TodoListDto>> GetTodoByUser(string userId)
        {
            return (await _valueProvider.GetValuesAsync())
                .Where(x => x.UserId == userId)
                .Select(x => new TodoListDto()
                {
                    NoteId = x.Id,
                    Description = x.Description,
                    GroupName = x.Group.Name,
                    StatusId = x.IsFinished
                });
        }



        [HttpPost]
        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> CreateToDoList(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(item);

                var dto = Convert(item);

                return CreatedAtRoute("DefaultApi", new { id = item.Id }, dto);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateToDo([FromBody]TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest(ModelState);
        }

        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> DeleteToDoList(int id)
        {
            var item = await _valueProvider.GetValueAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _valueProvider.DeleteValueAsync(id);
            return Ok();
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateToDoList([FromBody]IEnumerable<TodoItem> items)
        {
            if (items == null)
            {
                return BadRequest();
            }
            await _valueProvider.UpdateValuesAsync(items);
            return StatusCode(HttpStatusCode.NoContent);
        }

        private static TodoListDto Convert(TodoItem x)
        {
            return new TodoListDto()
            {
                NoteId = x.Id,
                Description = x.Description,
                GroupName = x.Group.Name,
                StatusId = x.IsFinished,
            };
        }
    }
}