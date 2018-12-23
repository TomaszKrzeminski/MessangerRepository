using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessengerApplication.WebUI.Infrastructure
{
    public class NinjectDependendcyResolver : IDependencyResolver
    {
        private IKernel kernel;


        public NinjectDependendcyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<IUserStatsRepository>().To<EFUserStatsRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}