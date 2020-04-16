using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using ToDoDAL.Model;
using ToDoProviders;
using ToDoWebAPI.Controllers;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Tests
{
    [TestClass]
    public class ToDoControllerTests
    {
        private readonly Mock<IEntityValueProvider<TodoItem>> _provider;
        private readonly IQueryable<TodoItem> _todoList = new List<TodoItem>()
        {
            new TodoItem() {Id = 1,Description = "",GroupName = "G1",IsFinished = false,Group = new TodoGroup(),UserId = "U1"},
            new TodoItem() {Id = 2,Description = "",GroupName = "G1",IsFinished = false,Group = new TodoGroup(),UserId = "U1"},
            new TodoItem() {Id = 3,Description = "",GroupName = "G1",IsFinished = false,Group = new TodoGroup(),UserId = "U1"}
        }.AsQueryable();

        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _provider = new Mock<IEntityValueProvider<TodoItem>>();
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
            Assert.AreEqual(1, contentresult.Content.NoteId);
        }

        [TestMethod]
        public async Task GetTodoListById_ReturnsNotFound()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(null);

            var result = await _controller.GetToDoList(1);
            var contentResult = result as NotFoundResult;

            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetTodoListByUser_ReturnsIQueryable()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(_todoList);

            var result = await _controller.GetTodoByUser("U1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<TodoListDto>));
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public async Task CreateTodoList_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("", "");

            var result = await _controller.CreateToDoList(new TodoItem());
            var contentResult = result as InvalidModelStateResult;

            Assert.IsInstanceOfType(contentResult, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task CreateTodoList_ReturnsCreatedAtRouteResult()
        {
            var result = await _controller.CreateToDoList(new TodoItem()
            {
                Id = 1,
                Description = "T1",
                GroupName = "G1",
                IsFinished = false,
                Group = new TodoGroup(),
                UserId = "U1"
            });
            var contentResult = result as CreatedAtRouteNegotiatedContentResult<TodoListDto>;

            _provider.Verify(m => m.CreateValueAsync(It.IsAny<TodoItem>()), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.NoteId, 1);
            Assert.AreEqual(contentResult.RouteName, "DefaultApi");
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsBadrequest()
        {
            _controller.ModelState.AddModelError("", "");

            var result = await _controller.UpdateToDo(new TodoItem());

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsNoContentRespons()
        {
            var result = await _controller.UpdateToDo(new TodoItem());
            var contentResult = result as StatusCodeResult;

            _provider.Verify(m => m.UpdateValueAsync(It.IsAny<TodoItem>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
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
            _provider.Setup(x => x.GetValueAsync(1)).ReturnsAsync(new TodoItem() { Id = 1 });

            var result = await _controller.DeleteToDoList(1);
            var contentResult = result as OkResult;

            _provider.Verify(m => m.DeleteValueAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsBadRequest()
        {
            var result = await _controller.UpdateToDoList(null);
            var contentResult = result as BadRequestResult;

            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public async Task UpdateTodoList_ReturnsNoContentStatus()
        {
            var result = await _controller.UpdateToDoList(_todoList);

            _provider.Verify(m => m.UpdateValuesAsync(It.IsAny<IEnumerable<TodoItem>>()), Times.Exactly(1));
            Assert.IsNotNull(result);
        }
    }
}
