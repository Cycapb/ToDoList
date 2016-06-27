﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public IEnumerable<Group> GetValues()
        {
           return _valueProvider.GetValues().ToList();
        }

        public Group GetGroup(int id)
        {
            return _valueProvider.GetValue(id);
        }

        [HttpPost]
        public HttpResponseMessage CreateGroup(Group group)
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
        public HttpResponseMessage UpdateGroup(Group item)
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
            try
            {
                _valueProvider.DeleteValue(id);
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
