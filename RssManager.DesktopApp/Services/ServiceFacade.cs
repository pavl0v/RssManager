using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace RssManager.DesktopApp.Services
{
    class ServiceFacade : IServiceFacade
    {
        public IServiceChannels ServiceChannels
        {
            get { return IoC.Container.Instance.Kernel.Get<IServiceChannels>(); }
        }

        public IServiceItems ServiceItems
        {
            get { return IoC.Container.Instance.Kernel.Get<IServiceItems>(); }
        }
    }
}
