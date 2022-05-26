using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ToDoBussinessLogic.Providers;
using ToDoDAL.Concrete;
using ToDoDAL.Model.MongoModel;
using ToDoDomainModels.Model;
using ToDoDomainModels.Repositories;
using ToDoProviders;

namespace ToDoWebAPI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;


        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IRepository<TodoGroup>>().To<EntityRepository<TodoGroup>>();
            _kernel.Bind<IRepository<TodoItem>>().To<EntityRepository<TodoItem>>();
            _kernel.Bind<IEntityValueProvider<TodoItem>>().To<TodoItemProvider>();
            _kernel.Bind<IEntityValueProvider<TodoGroup>>().To<TodoGroupProvider>();
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