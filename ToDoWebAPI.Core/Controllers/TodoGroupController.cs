﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Model;
using ToDoProviders;
using ToDoWebAPI.Core.Models;

namespace ToDoWebAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoGroupController : ControllerBase
    {
        private readonly IEntityValueProvider<TodoGroup> _valueProvider;

        public TodoGroupController(IEntityValueProvider<TodoGroup> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        [HttpGet("{id:int}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetGroup(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);

            if (group == null)
            {
                return NotFound();
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
        public async Task<IActionResult> GetValues(string userId)
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

            return Ok(todoGroupsDtos);
        }

        [HttpGet("{id:int}/todos")]
        public async Task<IActionResult> GetTodoLists(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);

            var items = group?.TodoItems.AsQueryable()
                .Select(x => new TodoItemDto()
                {
                    NoteId = x.Id,
                    Description = x.Description,
                    GroupName = x.Group.Name,
                    StatusId = x.IsFinished,
                });

            return Ok(items);

        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(TodoGroup group)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(group);
                return CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup(TodoGroup item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(int id)
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