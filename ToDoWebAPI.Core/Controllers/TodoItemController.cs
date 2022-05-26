using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ToDoDomainModels.Model;
using ToDoProviders;
using ToDoWebAPI.Core.Models;

namespace ToDoWebAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IEntityValueProvider<TodoItem> _entityValueProvider;

        public TodoItemsController(IEntityValueProvider<TodoItem> entityValueProvider)
        {
            _entityValueProvider = entityValueProvider;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var item = (await _entityValueProvider.GetValuesAsync())?
                .Where(x => x.Id == id)
                .Select(x => new TodoItemDto()
                {
                    Id = x.Id,
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

        [HttpGet("{userId}")]
        public async Task<IQueryable<TodoItemDto>> GetTodoItemByUser(string userId)
        {
            return (await _entityValueProvider.GetValuesAsync())
                .Where(x => x.UserId == userId)
                .Select(x => new TodoItemDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    GroupName = x.Group.Name,
                    StatusId = x.IsFinished
                });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _entityValueProvider.CreateValueAsync(item);

                var dto = Convert(item);

                return CreatedAtAction(nameof(CreateTodoItem), dto);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTodoItem([FromBody] TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _entityValueProvider.UpdateValueAsync(item);
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var item = await _entityValueProvider.GetValueAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await _entityValueProvider.DeleteValueAsync(id);
            return Ok();
        }

        private static TodoItemDto Convert(TodoItem x)
        {
            return new TodoItemDto()
            {
                Id = x.Id,
                Description = x.Description,
                GroupName = x.GroupName,
                StatusId = x.IsFinished,
            };
        }
    }
}