using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using RssManager.Interfaces.Repository;
using RssManager.WebAPI.Providers;
using RssManager.WebAPI.SignalR;
using System;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

[assembly: OwinStartup(typeof(RssManager.WebAPI.Startup))]
namespace RssManager.WebAPI
{
    public class Startup
    {
        private IKernel kernel = null;

        public void Configuration(IAppBuilder app)
        {
            string strPath = HttpContext.Current.Server.MapPath("~/log4net-config.xml");
            System.IO.FileInfo fi = new System.IO.FileInfo(strPath);
            log4net.Config.XmlConfigurator.Configure(fi);

            HttpConfiguration config = new HttpConfiguration();
            this.kernel = new StandardKernel(new IoC.NinjectModule1());
            config.DependencyResolver = new IoC.NinjectResolver(this.kernel);
            WebApiConfig.Register(config);

            int interval_min = 10; // minutes
            string strInterval = ConfigurationManager.AppSettings["RSSChannelUpdateInterval"];
            int.TryParse(strInterval, out interval_min);
            if (interval_min > 0)
            {
                int interval_sec = interval_min * 60 * 1000;
                BackgroundServerTimer timer = new BackgroundServerTimer(60000, interval_sec);
                timer.OnTimerElapsedHandler += timer_OnTimerElapsedHandler;
                HostingEnvironment.RegisterObject(timer);
            }

            ConfigureOAuth(app, kernel.Get<IUserRepository>());
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR();
        }

        public void ConfigureOAuth(IAppBuilder app, IUserRepository userRepository)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(userRepository)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void timer_OnTimerElapsedHandler(object sender, EventArgs e)
        {
            IRssChannelRepository rssChannelRepository = this.kernel.Get<IRssChannelRepository>();
            IRssItemRepository rssItemRepository = this.kernel.Get<IRssItemRepository>();
            IUserRepository userRepository = this.kernel.Get<IUserRepository>();
            ISettingsRepository settingsRepository = this.kernel.Get<ISettingsRepository>();
            RssInformer informer = new RssInformer(rssChannelRepository, rssItemRepository, userRepository, settingsRepository);
            informer.Broadcast();
        }
    }
}