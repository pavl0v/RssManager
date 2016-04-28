using Ninject;

namespace RssManager.DesktopApp.ViewModels
{
    public class ViewModelLocator
    {
        public SignInViewModel SignInViewModel
        {
            get 
            {
                return IoC.Container.Instance.Kernel.Get<SignInViewModel>();
            }
        }

        static ViewModelLocator()
        {

        }
    }
}
