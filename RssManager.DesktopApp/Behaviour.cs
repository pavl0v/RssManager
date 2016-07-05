using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace RssManager.DesktopApp
{
    public static class Behaviour
    {
        private static ListBox listbox = null;
        private static ScrollViewer scrollviewer = null;
        private static ScrollBar scrollbar = null;
        private static DispatcherTimer _resizeTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = false };
        private static DispatcherTimer _loadTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 100), IsEnabled = false };
        private static ICommand cmd = null;
        private static Window owner = null;
        //private static bool isScrollBarVisible = false;
        //private static MyDel myDel = null;

        #region Attached properties

        #region IsEnabled

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(Behaviour), new UIPropertyMetadata(false, isEnabledChangedCallback));

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        private static void isEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            listbox = d as ListBox;
            if (listbox == null)
                return;

            if (e.NewValue is bool && (bool)e.NewValue)
            {
                listbox.Loaded += listbox_Loaded;
                listbox.Unloaded += listbox_Unloaded;
                //_resizeTimer.Tick += _resizeTimer_Tick;
            }
            else
            {
                //listbox.Loaded -= listbox_Loaded;
                //_resizeTimer.Tick -= _resizeTimer_Tick;
            }
        }



        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Behaviour));

        public static ICommand GetCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        #endregion

        #region PageLoaded

        public static readonly DependencyProperty PageLoadedProperty =
            DependencyProperty.RegisterAttached("PageLoaded", typeof(bool), typeof(Behaviour),
                new FrameworkPropertyMetadata(false, PageLoadedChangedCallback));

        public static bool GetPageLoaded(DependencyObject element)
        {
            return (bool)element.GetValue(PageLoadedProperty);
        }

        public static void SetPageLoaded(DependencyObject element, bool value)
        {
            element.SetValue(PageLoadedProperty, value);
        }

        private static void PageLoadedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //if(scrollbar.IsVisible)
            //    return;

            //bool isScrollBarVisible = GetIsScrollBarVisible(d);
            if ((bool)e.OldValue == false && (bool)e.NewValue == true)
            {
                _loadTimer.Start();
                return;

                //Task t = new Task(() =>
                //{
                    
                //    double viewportHeight = scrollviewer.ViewportHeight == 0 ? listbox.Items.Count : scrollviewer.ViewportHeight;
                //    bool allPagesLoaded = GetAllPagesLoaded(d);
                //    //while ((!allPagesLoaded))
                //    //if(scrollviewer.VerticalOffset + viewportHeight >= listbox.Items.Count && listbox.Items.Count != 0)
                //    //{
                //        //System.Threading.Thread.Sleep(1000);
                //        //isScrollBarVisible = GetIsScrollBarVisible(d);
                //        LoadNextPage();
                //        allPagesLoaded = GetAllPagesLoaded(d);
                //    //}
                //    //if (scrollviewer.VerticalOffset + viewportHeight >= listbox.Items.Count && listbox.Items.Count != 0)
                //    //{
                //    //    LoadNextPage();
                //    //}

                //});
                //t.Start(TaskScheduler.FromCurrentSynchronizationContext());

                //double viewportHeight = 0;
                //Task t = new Task(() =>
                //{
                //    //LoadNextPage();
                //    viewportHeight = scrollviewer.ViewportHeight == 0 ? listbox.Items.Count : scrollviewer.ViewportHeight;
                //});
                //t.Start(TaskScheduler.FromCurrentSynchronizationContext());
                //if (myDel != null)
                //    myDel();

                //Task.Delay(500);
                //viewportHeight = scrollviewer.ViewportHeight == 0 ? listbox.Items.Count : scrollviewer.ViewportHeight;
                //System.Threading.Thread.Sleep(100);
                
                
                //Task t = new Task(() => 
                //{
                    
                //});
                //t.ContinueWith((x) => 
                //{
                //    System.Threading.Thread.Sleep(100);
                //    bool b = scrollbar.IsVisible;
                //});
                //t.Start(TaskScheduler.FromCurrentSynchronizationContext());
                
            }
        }

        #endregion

        #region AllPagesLoaded

        public static readonly DependencyProperty AllPagesLoadedProperty =
            DependencyProperty.RegisterAttached("AllPagesLoaded", typeof(bool), typeof(Behaviour),
                new FrameworkPropertyMetadata(false));

        public static bool GetAllPagesLoaded(DependencyObject element)
        {
            return (bool)element.GetValue(AllPagesLoadedProperty);
        }

        public static void SetAllPagesLoaded(DependencyObject element, bool value)
        {
            element.SetValue(AllPagesLoadedProperty, value);
        }

        #endregion

        #region ScrollBarVisibleCommand

        //public static readonly DependencyProperty ScrollBarVisibleCommandProperty =
        //    DependencyProperty.RegisterAttached("ScrollBarVisibleCommand", typeof(ICommand), typeof(Behaviour));

        //public static ICommand GetScrollBarVisibleCommand(DependencyObject element)
        //{
        //    return (ICommand)element.GetValue(ScrollBarVisibleCommandProperty);
        //}

        //public static void SetScrollBarVisibleCommand(DependencyObject element, ICommand value)
        //{
        //    element.SetValue(ScrollBarVisibleCommandProperty, value);
        //}

        #endregion

        #region IsScrollBarVisible

        //public static readonly DependencyProperty IsScrollBarVisibleProperty =
        //    DependencyProperty.RegisterAttached("IsScrollBarVisible", typeof(bool), typeof(Behaviour),
        //        new FrameworkPropertyMetadata(false));

        //public static bool GetIsScrollBarVisible(DependencyObject element)
        //{
        //    return (bool)element.GetValue(IsScrollBarVisibleProperty);
        //}

        //public static void SetIsScrollBarVisible(DependencyObject element, bool value)
        //{
        //    element.SetValue(IsScrollBarVisibleProperty, value);
        //}

        #endregion

        #endregion

        private static void _resizeTimer_Tick(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            #region Process RESIZED EVENT

            System.Diagnostics.Debug.WriteLine("RESIZED EVENT");
            if (scrollviewer.VerticalOffset + scrollviewer.ViewportHeight >= listbox.Items.Count && listbox.Items.Count != 0)
            {
                //System.Diagnostics.Debug.WriteLine("LAST");
                LoadNextPage();
            }

            #endregion
        }

        private static void listbox_Loaded(object sender, RoutedEventArgs e)
        {
            //myDel = CS;
            _resizeTimer.Tick += _resizeTimer_Tick;
            _loadTimer.Tick += _loadTimer_Tick;
            
            listbox = sender as ListBox;
            cmd = GetCommand(listbox);
            owner = GetFirstParentObjectOfType<Window>(listbox);

            scrollviewer = GetFirstChildObjectOfType<ScrollViewer>(listbox);
            scrollviewer.ScrollChanged += scrollviewer_ScrollChanged;
            scrollviewer.SizeChanged += scrollviewer_SizeChanged;
            
            

            scrollbar = GetFirstChildObjectOfType<ScrollBar>(listbox);
            scrollbar.IsVisibleChanged += scrollbar_IsVisibleChanged;
        }

        static void _loadTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (!scrollbar.IsVisible)
                LoadNextPage();
        }

        static void scrollbar_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            //isScrollBarVisible = !isScrollBarVisible;
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
                _loadTimer.Stop();
            //SetIsScrollBarVisible(listbox, (bool)e.NewValue);
            //scrollbar.IsVisibleChanged -= scrollbar_IsVisibleChanged;
        }

        static void listbox_Unloaded(object sender, RoutedEventArgs e)
        {
            _resizeTimer.Tick -= _resizeTimer_Tick;
            listbox.Loaded -= listbox_Loaded;
            listbox.Unloaded -= listbox_Unloaded;
        }

        static void scrollviewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!e.HeightChanged)
                return;

            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private static void LoadNextPage()
        {
            //System.Diagnostics.Debug.WriteLine("PLEASE LOAD NEXT PAGE");
            if (listbox != null)
            {
                //ICommand cmd = GetCommand(listbox);
                if (cmd != null)
                {
                    //Window owner = GetFirstParentObjectOfType<Window>(listbox);
                    // Hack to load dialog window content :Z
                    //Task t = new Task(() =>
                    //{
                    //    cmd.Execute(owner);
                    //});
                    //t.Start(TaskScheduler.FromCurrentSynchronizationContext());
                    cmd.Execute(owner);
                }
            }
        }

        private static void scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //ScrollViewer scrollviewer = sender as ScrollViewer;
            if (scrollviewer == null || scrollviewer.VerticalOffset == 0)
                return;

            if (scrollviewer.VerticalOffset == scrollviewer.ScrollableHeight)
            {
                LoadNextPage();
            }
        }

        //private delegate void MyDel();
        //private static void CS()
        //{
        //    if(!scrollbar.IsVisible)
        //        LoadNextPage();
        //}

        private static T GetFirstChildObjectOfType<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
                return default(T);

            T result = default(T);
            int qty = VisualTreeHelper.GetChildrenCount(d);

            for (int i = 0; i < qty; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, i);
                if (!(child is T))
                {
                    result = GetFirstChildObjectOfType<T>(child);
                }
                else
                {
                    result = child as T;
                    break;
                }
            }

            return result;
        }

        private static T GetFirstParentObjectOfType<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
                return default(T);

            T result = default(T);

            DependencyObject parent = VisualTreeHelper.GetParent(d);
            if (!(parent is T))
            {
                result = GetFirstParentObjectOfType<T>(parent);
            }
            else
            {
                result = parent as T;
            }

            return result;
        }
    }
}
