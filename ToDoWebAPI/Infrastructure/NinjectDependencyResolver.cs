using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using ToDoDAL.Abstract;
using ToDoDAL.Concrete;
using ToDoDAL.Model;

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