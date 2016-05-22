using System.Collections.Generic;
using System.Web.Http;
using ToDoDAL.Abstract;
using ToDoDAL.Model;


namespace ToDoWebAPI.Controllers
{
    public class ToDoController : ApiController
    {
        private readonly IRepository<ToDoList> _toDoRepository;

        public ToDoController(IRepository<ToDoList> toDoRepo)
        {
            _toDoRepository = toDoRepo;
        }

        public IEnumerable<ToDoList> GetToDoList()
        {
           return _toDoRepository.GetList();
        } 
    }
}