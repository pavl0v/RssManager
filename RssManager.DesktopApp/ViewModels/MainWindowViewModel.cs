using RssManager.DesktopApp.Dialogs;
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
        private int currentPageNo = 0;
        private long currentChannelId = 0;
        

        private List<RssChannelDTO> channels = null;
        public List<RssChannelDTO> Channels
        {
            get { return this.channels; }
            set { this.channels = value; }
        }

        private bool pageLoaded = false;
        public bool PageLoaded
        {
            get { return this.pageLoaded; }
            set 
            {
                if (this.pageLoaded != value)
                {
                    this.pageLoaded = value;
                    this.OnPropertyChanged("PageLoaded");
                }
            }
        }

        private bool allPagesLoaded = false;
        public bool AllPagesLoaded
        {
            get { return this.allPagesLoaded; }
            set
            {
                if (this.allPagesLoaded != value)
                {
                    this.allPagesLoaded = value;
                    this.OnPropertyChanged("AllPagesLoaded");
                }
            }
        }

        private bool isScrollBarVisible = false;
        public bool IsScrollBarVisible
        {
            get { return this.isScrollBarVisible; }
            set
            {
                if (this.isScrollBarVisible != value)
                {
                    this.isScrollBarVisible = value;
                    //this.OnPropertyChanged("IsScrollBarVisible");
                }
            }
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

        private ICommand scrollDownCommand = null;
        public ICommand ScrollDownCommand
        {
            get { return this.scrollDownCommand; }
            set { this.scrollDownCommand = value; }
        }

        private ICommand scrollBarVisibleCommand = null;
        public ICommand ScrollBarVisibleCommand
        {
            get { return this.scrollBarVisibleCommand; }
            set { this.scrollBarVisibleCommand = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade, IServiceFacade serviceFacade)
        {
            this.dialogFacade = dialogFacade;
            this.serviceFacade = serviceFacade;
            this.loadedCommand = new RelayCommand(OnLoaded);
            this.exitCommand = new RelayCommand(OnExit);
            this.channelSelectedCommand = new RelayCommand(OnChannelSelected);
            this.channelsReloadCommand = new RelayCommand(OnChannelsReload);
            this.scrollDownCommand = new RelayCommand(OnScrollDown);
            this.scrollBarVisibleCommand = new RelayCommand(OnScrollBarVisible);
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
            this.currentPageNo = 0;
            this.AllPagesLoaded = false;
            this.currentChannelId = Convert.ToInt64(parameter);
            this.Items.Clear();
            this.PageLoaded = false;
            //while (!this.isScrollBarVisible)
            {
                this.LoadNextPage();
            }
        }

        private void LoadNextPage()
        {
            this.PageLoaded = false;
            this.currentPageNo++;
            List<RssItemDTO> items = this.serviceFacade.ServiceItems.GetItems(this.currentChannelId, this.currentPageNo, 20);
            if (items == null || items.Count == 0)
                this.AllPagesLoaded = true;
            foreach (RssItemDTO item in items)
                this.Items.Add(item);
            this.PageLoaded = true;
        }

        private void OnScrollDown(object parameter)
        {
            //Window win = parameter as Window;
            //this.dialogFacade.ShowDialogOk("SCROLL", new DialogWindowProperties() { Owner = win });
            if (!this.AllPagesLoaded)
            {
                this.LoadNextPage();
            }
        }

        private void OnScrollBarVisible(object parameter)
        {
            this.PageLoaded = false;
        }

        private void OnChannelsReload(object parameter)
        {
            if(this.channels != null)
                this.channels.Clear();
            if (this.items != null)
                this.items.Clear();
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
