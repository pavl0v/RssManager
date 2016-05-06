using System;

namespace RssManager.DesktopApp.Services
{
    public interface IRequestApi
    {
        string Request(Method method, string url);
    }
}
