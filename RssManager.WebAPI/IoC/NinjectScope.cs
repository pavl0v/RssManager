using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace RssManager.WebAPI.IoC
{
    public class NinjectScope : IDependencyScope
    {
        // http://www.strathweb.com/2012/05/using-ninject-with-the-latest-asp-net-web-api-source/

        protected IResolutionRoot resolutionRoot;

        public NinjectScope(IResolutionRoot kernel)
        {
            resolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = (IDisposable)resolutionRoot;
            if (disposable != null) 
                disposable.Dispose();
            resolutionRoot = null;
        }
    }
}