using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using ToDoDAL.Abstract;
using ToDoDAL.Concrete;
using ToDoDAL.Model;
using ToDoDAL.Model.MongoModel;
using ToDoWebAPI.Abstract;
using ToDoWebAPI.Concrete;

namespace ToDoWebAPI.Infrastructure
{
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private readonly IKernel _kernel;
        

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IRepository<Group>>().To<EntityRepository<Group>>();
            _kernel.Bind<IRepository<ToDoList>>().To<EntityRepository<ToDoList>>();
            _kernel.Bind<IEntityValueProvider<ToDoList>>().To<ToDoListProvider>();
            _kernel.Bind<IEntityValueProvider<Group>>().To<GroupProvider>();
            _kernel.Bind<IMongoValueProvider<TaskGroup>>().To<MongoTaskGroupValueProvider>();
            _kernel.Bind<IMongoValueProvider<ToDoDAL.Model.MongoModel.Task>>().To<MongoTaskValueProvider>();
            _kernel.Bind<IMongoRepository<TaskGroup>>().To<MongoRepository<TaskGroup>>();
            _kernel.Bind<IMongoRepository<ToDoDAL.Model.MongoModel.Task>>().To<MongoRepository<ToDoDAL.Model.MongoModel.Task>>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}