using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoDAL.Model;

namespace ToDoWebAPI.Tests
{
    public class MockClass<T> where T : class
    {
        public EntityRepositoryFake<T> MockRepository;
        public Mock<DbSet<T>> MockDbSet;
        public Mock<TodoContext> MockContext;
    }

    [TestClass]
    public class EntityRepositoryTests
    {
        private List<TodoGroup> _groupList = new List<TodoGroup>()
        {
            new TodoGroup() {Id = 1, Name = "G1", UserId = "U1"},
            new TodoGroup() {Id = 2, Name = "G2", UserId = "U1"},
            new TodoGroup() {Id = 3, Name = "G3", UserId = "U1"},
            new TodoGroup() {Id = 4, Name = "G4", UserId = "U1"}
        };

        private List<TodoItem> _toDoList = new List<TodoItem>()
        {
            new TodoItem() {Id = 1, GroupId = 1, IsFinished = true, UserId = "U1"},
            new TodoItem() {Id = 2, GroupId = 1, IsFinished = true, UserId = "U1", Description = "N2"},
            new TodoItem() {Id = 3, GroupId = 2, IsFinished = false, UserId = "U1"},
            new TodoItem() {Id = 4, GroupId = 2, IsFinished = false, UserId = "U1"}
        };

        private MockClass<TodoItem> CreateToDoMockClass()
        {
            var toDoList = _toDoList.AsQueryable();
            var mockSet = new Mock<DbSet<TodoItem>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.Id == (int)ids[0]));
            mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(toDoList.Provider);
            mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(toDoList.Expression);
            mockSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(toDoList.ElementType);
            mockSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => toDoList.GetEnumerator());
            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.Set<TodoItem>()).Returns(mockSet.Object);
            return  new MockClass<TodoItem>()
            {
                MockContext = mockContext,
                MockDbSet = mockSet,
                MockRepository = new EntityRepositoryFake<TodoItem>(mockContext.Object)
            };
        }

        private MockClass<TodoGroup> CreateGroupMockClass()
        {
            var groupList = _groupList.AsQueryable();
            var mockSet = new Mock<DbSet<TodoGroup>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x=>x.Id == (int)ids[0]));
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.Id == (int)ids[0]));
            mockSet.As<IQueryable<TodoGroup>>().Setup(m => m.Provider).Returns(groupList.Provider);
            mockSet.As<IQueryable<TodoGroup>>().Setup(m => m.Expression).Returns(groupList.Expression);
            mockSet.As<IQueryable<TodoGroup>>().Setup(m => m.ElementType).Returns(groupList.ElementType);
            mockSet.As<IQueryable<TodoGroup>>().Setup(m => m.GetEnumerator()).Returns(() => groupList.GetEnumerator());
            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.Set<TodoGroup>()).Returns(mockSet.Object);
            return new MockClass<TodoGroup>()
            {
                MockDbSet = mockSet,
                MockContext = mockContext,
                MockRepository = new EntityRepositoryFake<TodoGroup>(mockContext.Object)
            };
        }

        [TestMethod]
        public async Task GetListAsyncReturnsList()
        {
            var repoTodo = CreateToDoMockClass();
            var repoGroup = CreateGroupMockClass();

            var targetToDo = (await repoTodo.MockRepository.GetListAsync()).ToList();
            var targetGroup = (await repoGroup.MockRepository.GetListAsync()).ToList();

            Assert.AreEqual(targetToDo[1].Id,2);
            Assert.AreEqual(targetToDo[1].Description,"N2");
            Assert.AreEqual(targetGroup[1].Name,"G2");
            Assert.AreEqual(targetGroup[1].UserId,"U1");
        }

        [TestMethod]
        public async Task GetItemAsyncReturnToDoOrGroupItem()
        {
            var repoTodo = CreateToDoMockClass();
            var repoGroup = CreateGroupMockClass();
            var itemId = 2;

            var targetToDo = await repoTodo.MockRepository.GetItemAsync(itemId);
            var targetGroup = await repoGroup.MockRepository.GetItemAsync(itemId);
            var targetToDoNull = await repoTodo.MockRepository.GetItemAsync(56);

            Assert.IsNull(targetToDoNull);
            Assert.AreEqual(targetToDo.Description,"N2");
            Assert.AreEqual(targetGroup.Name,"G2");
        }

        [TestMethod]
        public async Task CreateToDoList()
        {
            var mockClass = CreateToDoMockClass();
            var newTodoList = new TodoItem() {Id = 5,Description = "N5"};
            
            
            await mockClass.MockRepository.CreateAsync(newTodoList);
            mockClass.MockDbSet.Verify(m=>m.Add(newTodoList),Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsyncToDoList()
        {
            var mockClass = CreateToDoMockClass();
            var itemId = 2;

            await mockClass.MockRepository.DeleteAsync(itemId);

            mockClass.MockDbSet.Verify(m=>m.Remove(It.IsAny<TodoItem>()),Times.Once);
        }

        //[TestMethod]
        //public async Task UpdateAsyncToDoList()
        //{
        //    var toDoList = _toDoList.AsQueryable();
        //    var mockSet = new Mock<DbSet<TodoItem>>();
        //    mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
        //        .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.Id == (int)ids[0]));
        //    mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(toDoList.Provider);
        //    mockSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(toDoList.Expression);
        //    mockSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(toDoList.ElementType);
        //    mockSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => toDoList.GetEnumerator());
        //    var mockContext = new Mock<TodoContext>();
        //    mockContext.Setup(c => c.Set<TodoItem>()).Returns(mockSet.Object);
        //    var itemId = 2;
        //    Mock<EntityRepositoryFake<TodoItem>> mockRepo = new Mock<EntityRepositoryFake<TodoItem>>(mockContext.Object);
        //    mockRepo.Setup(m => m.UpdateAsync(It.IsAny<TodoItem>()));
        //    mockRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(mockSet.Object.Find(itemId));

        //    var newTodo = await mockRepo.Object.GetItemAsync(itemId);
        //    newTodo.Description = "N22";
        //    await mockRepo.Object.UpdateAsync(newTodo);
        //    var updatedItem = await mockRepo.Object.GetItemAsync(itemId);

        //    mockRepo.Verify(m=>m.UpdateAsync(newTodo),Times.Once);
            
        //    Assert.AreEqual(updatedItem.Description,"N22");
        //}

        [TestMethod]
        public async Task SaveAsyncToDoList()
        {
            var mockClass = CreateToDoMockClass();    
            await mockClass.MockRepository.SaveAsync();

            mockClass.MockContext.Verify(m=>m.SaveChangesAsync(),Times.Once);
        }
    }
}
