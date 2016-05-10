using RssManager.DesktopApp.Dialogs;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.DesktopApp.Services;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RssManager.DesktopApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDialogFacade dialogFacade = null;
        private IServiceFacade serviceFacade = null;

        private List<RssChannelDTO> channels = null;
        public List<RssChannelDTO> Channels
        {
            get { return this.channels; }
            set { this.channels = value; }
        }

        private ICommand loadedCommand = null;
        public ICommand LoadedCommand
        {
            get { return this.loadedCommand; }
            set { this.loadedCommand = value; }
        }

        private ICommand exitCommand = null;
        public ICommand ExitCommand
        {
            get { return this.exitCommand; }
            set { this.exitCommand = value; }
        }

        private ICommand channelsReloadCommand = null;
        public ICommand ChannelsReloadCommand
        {
            get { return this.channelsReloadCommand; }
            set { this.channelsReloadCommand = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade, IServiceFacade serviceFacade)
        {
            this.dialogFacade = dialogFacade;
            this.serviceFacade = serviceFacade;
            this.loadedCommand = new RelayCommand(OnLoaded);
            this.exitCommand = new RelayCommand(OnExit);
            this.channelsReloadCommand = new RelayCommand(OnChannelsReload);
        }

        private void OnLoaded(object parameter)
        {
            this.LoadChannels();
        }

        private void OnExit(object parameter)
        {
            Window win = parameter as Window;
            if (win != null)
                win.Close();
        }

        private void OnChannelsReload(object parameter)
        {
            if(this.channels != null)
                this.channels.Clear();
            this.LoadChannels();
        }

        private async void LoadChannels()
        {
            this.channels = await this.serviceFacade.ServiceChannels.GetChannelsAsync();
            this.OnPropertyChanged("Channels");
        }

        private void OnPropertyChanged(string propertName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertName));
        }
    }
}
