using RssManager.DesktopApp.Dialogs;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Ninject;

namespace RssManager.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Views.MainWindow mw = new Views.MainWindow();
            IDialogFacade df = IoC.Container.Instance.Kernel.Get<IDialogFacade>();
            DialogResult dr = df.ShowDialogSignIn(string.Empty, new DialogWindowProperties() { Title = "Authentication" });
            if (dr == DialogResult.Ok)
            {
                mw.Show();
            }
            else
                mw.Close();
        }
    }
}
