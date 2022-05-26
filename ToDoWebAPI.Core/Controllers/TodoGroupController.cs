using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDomainModels.Model;
using ToDoProviders;
using ToDoWebAPI.Core.Models;

namespace ToDoWebAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoGroupsController : ControllerBase
    {
        private readonly IEntityValueProvider<TodoGroup> _valueProvider;

        public TodoGroupsController(IEntityValueProvider<TodoGroup> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodoGroup(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);

            if (group == null)
            {
                return NoContent();
            }
            var groupDto = new TodoGroupDto()
            {
                GroupId = group.Id,
                Name = group.Name,
                UserId = group.UserId
            };
            return Ok(groupDto);
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<TodoGroupDto>> GetItems(string userId)
        {
            var todoGroups = await _valueProvider.GetValuesAsync();
            var todoGroupsDtos = todoGroups?
                .Where(x => x.UserId == userId)
                .Select(x => new TodoGroupDto()
                {
                    UserId = x.UserId,
                    GroupId = x.Id,
                    Name = x.Name
                });

            return todoGroupsDtos;
        }

        [HttpGet("{id:int}/todos")]
        public async Task<IEnumerable<TodoItemDto>> GetTodoItems(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);

            var items = group?.TodoItems.AsQueryable()
                .Select(x => new TodoItemDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    GroupName = x.Group.Name,
                    StatusId = x.IsFinished,
                });

            return items;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoGroup([FromBody] TodoGroup group)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(group);
                return CreatedAtAction(nameof(CreateTodoGroup), group);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTodoGroup([FromBody] TodoGroup item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoGroup(int id)
        {
            try
            {
                var item = await _valueProvider.GetValueAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                await _valueProvider.DeleteValueAsync(id);

                return Ok(item);
            }
            catch (Exception)
            {
                var message = "You cannot delete group while it has any ToDo items";

                return BadRequest(message);
            }
        }
    }
}