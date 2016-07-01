using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IEntityValueProvider<Group> _valueProvider;

        public GroupController(IEntityValueProvider<Group> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public async Task<IEnumerable<Group>> GetValues()
        {
           return (await _valueProvider.GetValuesAsync()).ToList();
        }

        public async Task<Group> GetGroup(int id)
        {
            return await _valueProvider.GetValueAsync(id);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.CreateValueAsync(group);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateGroup(Group item)
        {
            if (ModelState.IsValid)
            {
                await _valueProvider.UpdateValueAsync(item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public async Task<HttpResponseMessage> DeleteGroup(int id)
        {
            try
            {
                await _valueProvider.DeleteValueAsync(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var message = "You cannot delete group while it has any ToDo items";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,message);
            }
        }
    }
}
