using Ninject;

namespace RssManager.DesktopApp.ViewModels
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return IoC.Container.Instance.Kernel.Get<MainWindowViewModel>();
            }
        }

        static ViewModelLocator()
        {

        }
    }
}
