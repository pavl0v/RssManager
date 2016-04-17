using System;

namespace RssManager.Interfaces.BO
{
    public interface ISettings
    {
        string this[string key] { get; set; }
    }
}
