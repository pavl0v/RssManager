using System;
using System.Threading;
using System.Web.Hosting;

namespace RssManager.WebAPI
{
    public class BackgroundServerTimer : IRegisteredObject
    {
        // http://dontcodetired.com/blog/post/Using-Server-Side-Timers-and-SignalR-in-ASPNET-MVC-Applications.aspx

        public event EventHandler OnTimerElapsedHandler = null;
        private Timer timer;

        public BackgroundServerTimer(int delayStartby, int repeatEvery)
        {
            StartTimer(delayStartby, repeatEvery);
        }

        private void StartTimer(int delayStartby, int repeatEvery)
        {
            //int delayStartby = 30000;
            //int repeatEvery = 30000;
            this.timer = new Timer(TimerElapsed, null, delayStartby, repeatEvery);
        }

        private void TimerElapsed(object state)
        {
            // On timer elapsed
            if (this.OnTimerElapsedHandler != null)
                this.OnTimerElapsedHandler(this, EventArgs.Empty);
        }

        public void Stop(bool immediate)
        {
            this.timer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}