using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Interfaces.Repository
{
    public interface ISettingsRepository
    {
        ISettings Get();
    }
}
