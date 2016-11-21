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
        private List<Group> _groupList = new List<Group>()
        {
            new Group() {GroupId = 1, Name = "G1", UserId = "U1"},
            new Group() {GroupId = 2, Name = "G2", UserId = "U1"},
            new Group() {GroupId = 3, Name = "G3", UserId = "U1"},
            new Group() {GroupId = 4, Name = "G4", UserId = "U1"}
        };

        private List<ToDoList> _toDoList = new List<ToDoList>()
        {
            new ToDoList() {NoteId = 1, Name = "N1", GroupId = 1, StatusId = true, UserId = "U1"},
            new ToDoList() {NoteId = 2, Name = "N2", GroupId = 1, StatusId = true, UserId = "U1"},
            new ToDoList() {NoteId = 3, Name = "N3", GroupId = 2, StatusId = false, UserId = "U1"},
            new ToDoList() {NoteId = 4, Name = "N4", GroupId = 2, StatusId = false, UserId = "U1"}
        };

        private MockClass<ToDoList> CreateToDoMockClass()
        {
            var toDoList = _toDoList.AsQueryable();
            var mockSet = new Mock<DbSet<ToDoList>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.NoteId == (int)ids[0]));
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Provider).Returns(toDoList.Provider);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Expression).Returns(toDoList.Expression);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.ElementType).Returns(toDoList.ElementType);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.GetEnumerator()).Returns(() => toDoList.GetEnumerator());
            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.Set<ToDoList>()).Returns(mockSet.Object);
            return  new MockClass<ToDoList>()
            {
                MockContext = mockContext,
                MockDbSet = mockSet,
                MockRepository = new EntityRepositoryFake<ToDoList>(mockContext.Object)
            };
        }

        private MockClass<Group> CreateGroupMockClass()
        {
            var groupList = _groupList.AsQueryable();
            var mockSet = new Mock<DbSet<Group>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x=>x.GroupId == (int)ids[0]));
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.GroupId == (int)ids[0]));
            mockSet.As<IQueryable<Group>>().Setup(m => m.Provider).Returns(groupList.Provider);
            mockSet.As<IQueryable<Group>>().Setup(m => m.Expression).Returns(groupList.Expression);
            mockSet.As<IQueryable<Group>>().Setup(m => m.ElementType).Returns(groupList.ElementType);
            mockSet.As<IQueryable<Group>>().Setup(m => m.GetEnumerator()).Returns(() => groupList.GetEnumerator());
            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.Set<Group>()).Returns(mockSet.Object);
            return new MockClass<Group>()
            {
                MockDbSet = mockSet,
                MockContext = mockContext,
                MockRepository = new EntityRepositoryFake<Group>(mockContext.Object)
            };
        }

        [TestMethod]
        public async Task GetListAsyncReturnsList()
        {
            var repoTodo = CreateToDoMockClass();
            var repoGroup = CreateGroupMockClass();

            var targetToDo = (await repoTodo.MockRepository.GetListAsync()).ToList();
            var targetGroup = (await repoGroup.MockRepository.GetListAsync()).ToList();

            Assert.AreEqual(targetToDo[1].NoteId,2);
            Assert.AreEqual(targetToDo[1].Name,"N2");
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
            Assert.AreEqual(targetToDo.Name,"N2");
            Assert.AreEqual(targetGroup.Name,"G2");
        }

        [TestMethod]
        public async Task CreateToDoList()
        {
            var mockClass = CreateToDoMockClass();
            var newTodoList = new ToDoList() {NoteId = 5,Name = "N5"};
            
            
            await mockClass.MockRepository.CreateAsync(newTodoList);
            mockClass.MockDbSet.Verify(m=>m.Add(newTodoList),Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsyncToDoList()
        {
            var mockClass = CreateToDoMockClass();
            var itemId = 2;

            await mockClass.MockRepository.DeleteAsync(itemId);

            mockClass.MockDbSet.Verify(m=>m.Remove(It.IsAny<ToDoList>()),Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsyncToDoList()
        {
            var toDoList = _toDoList.AsQueryable();
            var mockSet = new Mock<DbSet<ToDoList>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => mockSet.Object.FirstOrDefault(x => x.NoteId == (int)ids[0]));
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Provider).Returns(toDoList.Provider);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.Expression).Returns(toDoList.Expression);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.ElementType).Returns(toDoList.ElementType);
            mockSet.As<IQueryable<ToDoList>>().Setup(m => m.GetEnumerator()).Returns(() => toDoList.GetEnumerator());
            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.Set<ToDoList>()).Returns(mockSet.Object);
            var itemId = 2;
            Mock<EntityRepositoryFake<ToDoList>> mockRepo = new Mock<EntityRepositoryFake<ToDoList>>(mockContext.Object);
            mockRepo.Setup(m => m.UpdateAsync(It.IsAny<ToDoList>()));
            mockRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).Returns(mockSet.Object.FindAsync(itemId));

            var newTodo = await mockRepo.Object.GetItemAsync(itemId);
            newTodo.Name = "N22";
            await mockRepo.Object.UpdateAsync(newTodo);
            var updatedItem = await mockRepo.Object.GetItemAsync(itemId);

            mockRepo.Verify(m=>m.UpdateAsync(newTodo),Times.Once);
            
            Assert.AreEqual(updatedItem.Name,"N22");
        }

        [TestMethod]
        public async Task SaveAsyncToDoList()
        {
            var mockClass = CreateToDoMockClass();    
            await mockClass.MockRepository.SaveAsync();

            mockClass.MockContext.Verify(m=>m.SaveChangesAsync(),Times.Once);
        }
    }
}
