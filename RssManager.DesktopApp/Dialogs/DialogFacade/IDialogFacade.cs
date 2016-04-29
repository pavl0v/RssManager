using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RssManager.DesktopApp.Dialogs.DialogFacade
{
    public interface IDialogFacade
    {
        DialogResult ShowDialogOk(string message, DialogWindowProperties properties);
        DialogResult ShowDialogSignIn(string message, DialogWindowProperties properties);
    }
}
