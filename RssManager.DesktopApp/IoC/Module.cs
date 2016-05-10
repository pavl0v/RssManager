using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.DesktopApp.ViewModels;
using RssManager.DesktopApp.Services;

namespace RssManager.DesktopApp.IoC
{
    class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IDialogFacade>().To<DialogFacade>().InSingletonScope();
            Bind<IRequestApi>().To<RequestApi>().InSingletonScope();
            Bind<IServiceChannels>().To<ServiceChannels>().InSingletonScope();
            Bind<IServiceItems>().To<ServiceItems>().InSingletonScope();
            Bind<IServiceFacade>().To<ServiceFacade>().InSingletonScope();
            Bind<MainWindowViewModel>().ToSelf();
        }
    }
}
