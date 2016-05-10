﻿using RssManager.DesktopApp.Dialogs;
using RssManager.DesktopApp.Dialogs.DialogFacade;
using RssManager.DesktopApp.Services;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<RssItemDTO> items = new ObservableCollection<RssItemDTO>();
        public ObservableCollection<RssItemDTO> Items
        {
            get { return this.items; }
            set { this.items = value; }
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

        private ICommand channelSelectedCommand = null;
        public ICommand ChannelSelectedCommand
        {
            get { return this.channelSelectedCommand; }
            set { this.channelSelectedCommand = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade, IServiceFacade serviceFacade)
        {
            this.dialogFacade = dialogFacade;
            this.serviceFacade = serviceFacade;
            this.loadedCommand = new RelayCommand(OnLoaded);
            this.exitCommand = new RelayCommand(OnExit);
            this.channelSelectedCommand = new RelayCommand(OnChannelSelected);
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

        private void OnChannelSelected(object parameter)
        {
            //this.dialogFacade.ShowDialogOk(parameter.ToString(), new DialogWindowProperties());
            long channelId = Convert.ToInt64(parameter);
            List<RssItemDTO> items = this.serviceFacade.ServiceItems.GetItems(channelId, 1, 20);
            this.Items.Clear();
            foreach (RssItemDTO item in items)
                this.Items.Add(item);
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
