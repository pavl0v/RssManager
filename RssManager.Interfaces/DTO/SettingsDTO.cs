using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Interfaces.DTO
{
    public class SettingsDTO : ISettingsDTO
    {
        public bool AutoRefresh
        {
            get { return false; }
        }
    }
}
