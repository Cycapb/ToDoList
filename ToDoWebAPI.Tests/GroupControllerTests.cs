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
    public class GroupControllerTests
    {
        private readonly Mock<IEntityValueProvider<Group>> _provider;
        private readonly GroupController _controller;
        private readonly IQueryable<Group> _groups = new List<Group>()
        {
            new Group()
            {
                GroupId = 1, UserId = "1",Name = "G1",ToDoList = new List<ToDoList>()
            },
            new Group() {GroupId = 2, UserId = "1",Name = "G2",ToDoList = new List<ToDoList>()},
            new Group() {GroupId = 3, UserId = "2",Name = "G3",ToDoList = new List<ToDoList>()},
            new Group() {GroupId = 4, UserId = "3",Name = "G4",ToDoList = new List<ToDoList>()},
        }.AsQueryable();

        public GroupControllerTests()
        {
            _provider = new Mock<IEntityValueProvider<Group>>();
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
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new Group() {GroupId = 10});

            var result = await _controller.GetGroup(It.IsAny<int>());
            var contentResult = result as OkNegotiatedContentResult<GroupDto>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.GroupId,10);
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
            Assert.AreEqual(result.Count(),2);
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
            _provider.Setup(m => m.GetValueAsync(groupId)).ReturnsAsync(new Group()
            {
                ToDoList = new List<ToDoList>()
                {
                    new ToDoList() {Group = new Group(),Comment = "",GroupId = 1,GroupName = "G1",Name = "T1",NoteId = 1,StatusId = false,UserId = "U1"},
                    new ToDoList() {Group = new Group(),Comment = "",GroupId = 1,GroupName = "G1",Name = "T2",NoteId = 2,StatusId = false,UserId = "U1"}
                }
            });

            var result = await _controller.GetTodoLists(groupId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(),2);
        }

        [TestMethod]
        public async Task UpdateGroup_ReturnsBadRequestWithModelState()
        {
            _controller.ModelState.AddModelError("","");

            var result = await _controller.UpdateGroup(new Group());

            Assert.IsInstanceOfType(result,typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task UpdateGroup_ReturnsStatucCodeNoContent()
        {

            var result = await _controller.UpdateGroup(It.IsAny<Group>());
            var contentResult = result as StatusCodeResult;

            _provider.Verify(m => m.UpdateValueAsync(It.IsAny<Group>()), Times.Exactly(1));    
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
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new Group());
            _provider.Setup(m => m.DeleteValueAsync(It.IsAny<int>())).Throws<System.Exception>();

            var result = await _controller.DeleteGroup(1);
            var contentResult = result as BadRequestErrorMessageResult;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Message);
        }

        [TestMethod]
        public async Task DeleteGroup_ReturnsStatusOk()
        {
            _provider.Setup(m => m.GetValueAsync(It.IsAny<int>())).ReturnsAsync(new Group() {GroupId = 1});

            var result = await _controller.DeleteGroup(It.IsAny<int>());
            var contentResult = result as OkNegotiatedContentResult<Group>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.GroupId, 1);
        }

        [TestMethod]
        public async Task CreateGroup_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("","");

            var result = await _controller.CreateGroup(new Group());
            var contentResult = result as InvalidModelStateResult;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.ModelState.Keys.Count,1);
        }

        [TestMethod]
        public async Task CreateGroup_ReturnsCreateAtRouteResult()
        {
            var result = await _controller.CreateGroup(new Group() {GroupId = 1});
            var contentResult = result as CreatedAtRouteNegotiatedContentResult<Group>;

            _provider.Verify(m => m.CreateValueAsync(It.IsAny<Group>()), Times.Exactly(1));
            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.RouteName,"DefaultApi");
            Assert.AreEqual(contentResult.Content.GroupId,1);
        }
    }
}
