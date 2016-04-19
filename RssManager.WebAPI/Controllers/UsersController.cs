using log4net;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Repository;
using RssManager.Interfaces.RssContentReader;
using RssManager.Objects.BO;
using RssManager.Objects.RssContentReader;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RssManager.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UsersController));
        private IUserRepository userRepository = null;
        private IRssChannelRepository rssChannelRepository = null;
        private IRssItemRepository rssItemRepository = null;

        public UsersController(IUserRepository userRepository, IRssChannelRepository rssChannelRepository, IRssItemRepository rssItemRepository)
        {
            this.userRepository = userRepository;
            this.rssChannelRepository = rssChannelRepository;
            this.rssItemRepository = rssItemRepository;
        }

        public IUserDTO Get(string username)
        {
            /**
             * Get user by username
             */

            User user = null;
            try
            {
                user = this.userRepository.GetByUsername(username) as User;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                throw new HttpResponseException(message);
            }

            return user;
        }

        [HttpPost]
        public void Post([FromBody]UserDTO user)
        {
            /**
             * Create new user
             */

            if (user == null)
            {
                string msg = "User is Null";
                log.Error(msg);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(msg) };
                throw new HttpResponseException(message);
            }

            if (string.IsNullOrEmpty(user.FirstName) ||
                string.IsNullOrEmpty(user.LastName) ||
                string.IsNullOrEmpty(user.UserName) ||
                string.IsNullOrEmpty(user.Password))
            {
                string msg = "All fields are required";
                log.Error(msg);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(msg) };
                throw new HttpResponseException(message);
            }

            Guid guid = Guid.NewGuid();
            user.Guid = guid.ToString("N");
            user.Password = Helper.GetHashedString(user.Password + user.Guid);

            try
            {
                string defaultUrl = ConfigurationManager.AppSettings["DefaultRSSChannelURL"];
                string defaultName = ConfigurationManager.AppSettings["DefaultRSSChannelName"];

                User bo = new User(user);
                this.userRepository.Save(bo);

                //
                IRssSourceContentReader reader = new RssHttpContentReader(defaultUrl);
                RssChannel channel = new RssChannel(reader, this.rssItemRepository);
                channel.Autorefresh = true;
                channel.Name = defaultName;
                channel.Refresh();
                this.rssChannelRepository.AddByUserId(channel, bo.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                throw new HttpResponseException(message);
            }
        }

        [HttpPut]
        [Authorize]
        public void Password(string oldpwd, string newpwd)
        {
            long userId = Helper.GetCurrentUserID(User);
            User user = null;
            try
            {
                user = this.userRepository.Get(userId) as User;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                throw new HttpResponseException(message);
            }

            if (user == null)
            {
                string msg = "User is NULL";
                log.Error(msg);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(msg) };
                throw new HttpResponseException(message);
            }

            if (user.Password != Helper.GetHashedString(oldpwd + user.Guid))
            {
                string msg = "Current password is incorrect";
                log.Error(msg);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(msg) };
                throw new HttpResponseException(message);
            }

            try
            {
                user.Password = Helper.GetHashedString(newpwd + user.Guid);
                this.userRepository.Save(user);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                throw new HttpResponseException(message);
            }
        }
    }
}
