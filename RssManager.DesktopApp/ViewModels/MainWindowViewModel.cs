using RssManager.DesktopApp.Dialogs;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RssManager.DesktopApp.ViewModels
{
    public class MainWindowViewModel
    {
        private IDialogFacade dialogFacade = null;

        private ICommand cmd = null;
        public ICommand Cmd
        {
            get { return this.cmd; }
            set { this.cmd = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade)
        {
            this.dialogFacade = dialogFacade;
            this.cmd = new RelayCommand(OnCmd);
        }

        private void OnCmd(object parameter)
        {
            this.dialogFacade.ShowDialogOk("Test message", new DialogWindowProperties() { Owner = parameter as Window });
        }
    }
}
