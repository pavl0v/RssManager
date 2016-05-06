using RssManager.DesktopApp.Dialogs;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.DesktopApp.Services;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RssManager.DesktopApp.ViewModels
{
    public class MainWindowViewModel
    {
        private IDialogFacade dialogFacade = null;
        private IServiceFacade serviceFacade = null;

        private ICommand cmd = null;
        public ICommand Cmd
        {
            get { return this.cmd; }
            set { this.cmd = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade, IServiceFacade serviceFacade)
        {
            this.dialogFacade = dialogFacade;
            this.serviceFacade = serviceFacade;
            this.cmd = new RelayCommand(OnCmd);
        }

        private void OnCmd(object parameter)
        {
            //this.dialogFacade.ShowDialogOk("Test message", new DialogWindowProperties() { Owner = parameter as Window });
            List<RssChannelDTO> s = this.serviceFacade.ServiceChannels.GetChannels();
        }
    }
}
