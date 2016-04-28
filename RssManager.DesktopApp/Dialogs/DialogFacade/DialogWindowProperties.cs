using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RssManager.DesktopApp.Dialogs.DialogFacade
{
    public class DialogWindowProperties
    {
        public Window Owner { get; set; }
        public ResizeMode ResizeMode { get; set; }
        public string Title { get; set; }

        public DialogWindowProperties()
        {
            this.Owner = null;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.Title = "Dialog";
        }
    }
}
