using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.DesktopApp.ViewModels;

namespace RssManager.DesktopApp.IoC
{
    class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IDialogFacade>().To<DialogFacade>().InSingletonScope();
            Bind<MainWindowViewModel>().ToSelf();
        }
    }
}
