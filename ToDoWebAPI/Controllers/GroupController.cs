﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoDAL.Model.MongoModel;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IMongoValueProvider<TaskGroup> _valueProvider;

        public GroupController(IMongoValueProvider<TaskGroup> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public IEnumerable<TaskGroup> GetValues()
        {
            return _valueProvider.GetValues();
        }

        public TaskGroup GetGroup(int id)
        {
            return _valueProvider.GetValue(id);
        }

        [HttpPost]
        public HttpResponseMessage CreateGroup(TaskGroup group)
        {
            if (ModelState.IsValid)
            {
                _valueProvider.CreateValue(group);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateGroup(TaskGroup item)
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

        public HttpResponseMessage DeleteGroup(int id)
        {
            _valueProvider.DeleteValue(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
