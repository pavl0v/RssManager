using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RssManager.DesktopApp.Dialogs.DialogFacade
{
    public interface IDialogFacade
    {
        DialogResult ShowDialogYesNo(string message, DialogWindowProperties properties);
    }
}
