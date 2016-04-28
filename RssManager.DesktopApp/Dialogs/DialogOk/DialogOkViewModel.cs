using RssManager.DesktopApp.Dialogs.DialogFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RssManager.DesktopApp.Dialogs.DialogOk
{
    class DialogOkViewModel : DialogViewModelBase
    {
        private ICommand okCommand = null;
        public ICommand OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
        }

        public DialogOkViewModel(string message)
            : base(message)
        {
            this.okCommand = new RelayCommand(OnOkClicked);
        }

        private void OnOkClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Ok);
        }
    }
}
