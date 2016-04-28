using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RssManager.DesktopApp.Dialogs.DialogFacade
{
    public abstract class DialogViewModelBase
    {
        public DialogResult UserDialogResult
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public DialogViewModelBase(string message)
        {
            this.Message = message;
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result)
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;
        }
    }
}
