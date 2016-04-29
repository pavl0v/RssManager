using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RssManager.DesktopApp.Dialogs.DialogFacade
{
    public class DialogFacade : IDialogFacade
    {
        public DialogFacade()
        {

        }

        public DialogResult ShowDialogOk(string message, DialogWindowProperties properties)
        {
            DialogViewModelBase vm = new DialogOk.DialogOkViewModel(message);
            return this.ShowDialog(vm, properties);
        }

        public DialogResult ShowDialogSignIn(string message, DialogWindowProperties properties)
        {
            DialogViewModelBase vm = new DialogSignIn.DialogSignInViewModel(message);
            return this.ShowDialog(vm, properties);
        }

        private DialogResult ShowDialog(DialogViewModelBase vm, DialogWindowProperties properties)
        {
            DialogWindow win = new DialogWindow();

            if (properties != null)
            {
                win.ResizeMode = properties.ResizeMode;
                win.Title = properties.Title;
                win.Owner = properties.Owner;
            }

            if (win.Owner == null)
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            win.DataContext = vm;
            win.ShowDialog();
            DialogResult result =
                (win.DataContext as DialogViewModelBase).UserDialogResult;
            return result;
        }
    }
}
