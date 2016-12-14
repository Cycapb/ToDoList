using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers
{
    [RoutePrefix("api/group")]
    public class GroupController : ApiController
    {
        private readonly IEntityValueProvider<Group> _valueProvider;

        public GroupController(IEntityValueProvider<Group> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        [Route("{id:int}")]
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            var groupDto = new GroupDto()
            {
                GroupId = group.GroupId,
                Name = group.Name,
                UserId = group.UserId
            };
            return Ok(groupDto);
        }

        [Route("{userId}")]
        public async Task<IQueryable<GroupDto>> GetValues(string userId)
        {
            return (await _valueProvider.GetValuesAsync())?
                .Where(x => x.UserId == userId)
                .Select(x => new GroupDto()
                {
                    UserId = x.UserId,
                    GroupId = x.GroupId,
                    Name = x.Name
                });
        }

        [Route("{id:int}/todos")]
        public async Task<IQueryable<TodoListDto>> GetTodoLists(int id)
        {
            var group = await _valueProvider.GetValueAsync(id);

            var items = group?.ToDoList.AsQueryable()
                .Select(x => new TodoListDto()
                {
                    NoteId = x.NoteId,
                    Comment = x.Comment,
                    GroupName = x.Group.Name,
                    Name = x.Name,
                    StatusId = x.StatusId,
                });
            return items;

        }

        [HttpPost]
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> CreateGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(group);
                return CreatedAtRoute("DefaultApi",new {id = group.GroupId}, group);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateGroup(Group item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest(ModelState);
        }

        public async Task<IHttpActionResult> DeleteGroup(int id)
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
