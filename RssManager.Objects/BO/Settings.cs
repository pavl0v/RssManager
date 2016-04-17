using RssManager.Interfaces.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Objects.BO
{
    public class Settings : ISettings
    {
        private Dictionary<string, string> settings = null;

        public string this[string key] {
            get { return this.settings[key]; }
            set { this.settings[key] = value; }
        }

        public Settings(Dictionary<string, string> settings)
        {
            this.settings = settings;
        }
    }
}
