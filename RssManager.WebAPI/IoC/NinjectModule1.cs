using Ninject.Modules;
using RssManager.Interfaces.Repository;
using RssManager.Repository.ADO;

namespace RssManager.WebAPI.IoC
{
    public class NinjectModule1 : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRssItemRepository>().To<RssItemRepository>();
            this.Bind<IRssChannelRepository>().To<RssChannelRepository>();
            this.Bind<IUserRepository>().To<UserRepository>();
            this.Bind<ISettingsRepository>().To<SettingsRepository>();
        }
    }
}