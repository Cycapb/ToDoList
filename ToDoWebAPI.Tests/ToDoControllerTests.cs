using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;
using ToDoWebAPI.Controllers;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Tests
{
    [TestClass]
    public class ToDoControllerTests
    {
        private readonly Mock<IEntityValueProvider<ToDoList>> _provider;
        private readonly IQueryable<ToDoList> _todoList = new List<ToDoList>()
        {
            new ToDoList() {NoteId = 1,Comment = "",GroupName = "G1",Name = "T1",StatusId = false,Group = new Group(),UserId = "U1"},
            new ToDoList() {NoteId = 2,Comment = "",GroupName = "G1",Name = "T2",StatusId = false,Group = new Group(),UserId = "U1"},
            new ToDoList() {NoteId = 3,Comment = "",GroupName = "G1",Name = "T3",StatusId = false,Group = new Group(),UserId = "U1"}
        }.AsQueryable();

        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _provider = new Mock<IEntityValueProvider<ToDoList>>();
            _controller = new ToDoController(_provider.Object);
        }

        [TestMethod]
        public async Task GetToDoListById_ReturnsOk()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(_todoList);

            var result = await _controller.GetToDoList(1);
            var contentresult = result as OkNegotiatedContentResult<TodoListDto>;

            Assert.IsNotNull(contentresult);
            Assert.IsNotNull(contentresult.Content);
            Assert.AreEqual(1,contentresult.Content.NoteId);
        }

        [TestMethod]
        public async Task GetTodoListById_ReturnsNotFound()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(null);

            var result = await _controller.GetToDoList(1);
            var contentResult = result as NotFoundResult;

            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult,typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetTodoListByUser_ReturnsIQueryable()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(_todoList);

            var result = await _controller.GetTodoByUser("U1");
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<TodoListDto>));
            Assert.AreEqual(3,result.Count());
        }

        [TestMethod]
        public async Task CreateTodoList_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("", "");

            var result = await _controller.CreateToDoList(new ToDoList());
            var contentResult = result as InvalidModelStateResult;

            Assert.IsInstanceOfType(contentResult, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task CreateTodoList_ReturnsCreatedAtRouteResult()
        {
            var result = await _controller.CreateToDoList(new ToDoList()
            {
                NoteId = 1,
                Comment = "",
                GroupName = "G1",
                Name = "T1",
                StatusId = false,
                Group = new Group(),
                UserId = "U1"
            });
            var contentResult = result as CreatedAtRouteNegotiatedContentResult<TodoListDto>;

            _provider.Verify(m => m.CreateValueAsync(It.IsAny<ToDoList>()), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.NoteId,1);
            Assert.AreEqual(contentResult.RouteName,"DefaultApi");
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsBadrequest()
        {
            _controller.ModelState.AddModelError("","");

            var result = await _controller.UpdateToDo(1, new ToDoList());

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsNoContentRespons()
        {
            var result = await _controller.UpdateToDo(1, new ToDoList());
            var contentResult = result as StatusCodeResult;

            _provider.Verify(m => m.UpdateValueAsync(It.IsAny<ToDoList>()),Times.Exactly(1));
            Assert.IsInstanceOfType(result,typeof(StatusCodeResult));
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFoundIfNull()
        {
            _provider.Setup(x => x.GetValueAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await _controller.DeleteToDoList(1);
            var contentResult = result as NotFoundResult;

            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOkNegotiatedContentResult()
        {
            _provider.Setup(x => x.GetValueAsync(1)).ReturnsAsync(new ToDoList() {NoteId = 1});

            var result = await _controller.DeleteToDoList(1);
            var contentResult = result as OkResult;

            _provider.Verify(m => m.DeleteValueAsync(It.IsAny<int>()),Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult, typeof(OkResult));
        }

    }
}
