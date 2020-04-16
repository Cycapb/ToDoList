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
    public class GroupControllerTests
    {
        private readonly Mock<IEntityValueProvider<TodoGroup>> _provider;
        private readonly GroupController _controller;
        private readonly IQueryable<TodoGroup> _groups = new List<TodoGroup>()
        {
            new TodoGroup()
            {
                Id = 1, UserId = "1",Name = "G1",TodoItems = new List<TodoItem>()
            },
            new TodoGroup() {Id = 2, UserId = "1",Name = "G2",TodoItems = new List<TodoItem>()},
            new TodoGroup() {Id = 3, UserId = "2",Name = "G3",TodoItems = new List<TodoItem>()},
            new TodoGroup() {Id = 4, UserId = "3",Name = "G4",TodoItems = new List<TodoItem>()},
        }.AsQueryable();

        public GroupControllerTests()
        {
            _provider = new Mock<IEntityValueProvider<TodoGroup>>();
            _controller = new GroupController(_provider.Object);
        }

        [TestMethod]
        public async Task GetGroup_ReturnsNotFoundResult()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await _controller.GetGroup(1);
            var contentResult = result as NotFoundResult;

            _provider.Verify(m => m.GetValueAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public async Task GetGroup_ReturnsOkNegotiationResult()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new TodoGroup() { Id = 10 });

            var result = await _controller.GetGroup(It.IsAny<int>());
            var contentResult = result as OkNegotiatedContentResult<GroupDto>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.GroupId, 10);
        }

        [TestMethod]
        public async Task GetValues_ReturnsNull()
        {
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(null);

            var result = await _controller.GetValues("1");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetValues_Returns2GroupDTOs()
        {
            var userId = "1";
            _provider.Setup(m => m.GetValuesAsync()).ReturnsAsync(_groups.Where(x => x.UserId == userId));

            var result = await _controller.GetValues(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
        }

        [TestMethod]
        public async Task GetTodoLists_ReturnsNull()
        {
            _provider.Setup(x => x.GetValueAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await _controller.GetTodoLists(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetTodoLists_ReturnsTodoListsQueryable()
        {
            var groupId = 1;
            _provider.Setup(m => m.GetValueAsync(groupId)).ReturnsAsync(new TodoGroup()
            {
                TodoItems = new List<TodoItem>()
                {
                    new TodoItem() {Group = new TodoGroup(),Description = "",GroupId = 1,GroupName = "G1",Id = 1,IsFinished = false,UserId = "U1"},
                    new TodoItem() {Group = new TodoGroup(),Description = "",GroupId = 1,GroupName = "G1",Id = 2,IsFinished = false,UserId = "U1"}
                }
            });

            var result = await _controller.GetTodoLists(groupId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
        }

        [TestMethod]
        public async Task UpdateGroup_ReturnsBadRequestWithModelState()
        {
            _controller.ModelState.AddModelError("", "");

            var result = await _controller.UpdateGroup(new TodoGroup());

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task UpdateGroup_ReturnsStatucCodeNoContent()
        {

            var result = await _controller.UpdateGroup(It.IsAny<TodoGroup>());
            var contentResult = result as StatusCodeResult;

            _provider.Verify(m => m.UpdateValueAsync(It.IsAny<TodoGroup>()), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task DeleteGroup_ReturnsNotFoundResult()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await _controller.DeleteGroup(It.IsAny<int>());
            var contentResult = result as NotFoundResult;

            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(contentResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteGroup_ReturnsBadRequestException()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new TodoGroup());
            _provider.Setup(m => m.DeleteValueAsync(It.IsAny<int>())).Throws<System.Exception>();

            var result = await _controller.DeleteGroup(1);
            var contentResult = result as BadRequestErrorMessageResult;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Message);
        }

        [TestMethod]
        public async Task DeleteGroup_ReturnsStatusOk()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new TodoGroup() { Id = 1 });

            var result = await _controller.DeleteGroup(It.IsAny<int>());
            var contentResult = result as OkNegotiatedContentResult<TodoGroup>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.Id, 1);
        }

        [TestMethod]
        public async Task CreateGroup_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("", "");

            var result = await _controller.CreateGroup(new TodoGroup());
            var contentResult = result as InvalidModelStateResult;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.ModelState.Keys.Count, 1);
        }

        [TestMethod]
        public async Task CreateGroup_ReturnsCreateAtRouteResult()
        {
            var result = await _controller.CreateGroup(new TodoGroup() { Id = 1 });
            var contentResult = result as CreatedAtRouteNegotiatedContentResult<TodoGroup>;

            _provider.Verify(m => m.CreateValueAsync(It.IsAny<TodoGroup>()), Times.Exactly(1));
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.RouteName, "DefaultApi");
            Assert.AreEqual(contentResult.Content.Id, 1);
        }
    }
}
