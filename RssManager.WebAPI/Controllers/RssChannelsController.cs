using log4net;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Repository;
using RssManager.Interfaces.RssContentReader;
using RssManager.Objects.BO;
using RssManager.Objects.RssContentReader;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RssManager.WebAPI.Controllers
{
    public class RssChannelsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssChannelsController));
        private IRssChannelRepository rssChannelRepository = null;
        private IRssItemRepository rssItemRepository = null;

        public RssChannelsController(IRssChannelRepository rssChannelRepository, IRssItemRepository rssItemRepository)
        {
            this.rssChannelRepository = rssChannelRepository;
            this.rssItemRepository = rssItemRepository;
        }

        // GET api/RssChannels
        [Authorize]
        public IEnumerable<IRssChannelDTO> Get()
        {
            long userId = Helper.GetCurrentUserID(User);
            IEnumerable<IRssChannelDTO> lst = this.rssChannelRepository.GetByUserId(userId);
            return lst;
        }

        // GET api/RssChannels/5
        public IRssChannelDTO Get(long id)
        {
            long userId = Helper.GetCurrentUserID(User);
            IRssChannelDTO dto = this.rssChannelRepository.GetDTO(id, userId);
            return dto;
        }

        // POST api/RssChannels
        public void Post([FromBody]RssChannelDTO channelDTO)
        {
            // Create new channel
            if (channelDTO == null)
                return;

            IRssSourceContentReader reader = new RssHttpContentReader(channelDTO.Url);
            RssChannel channelBO = new RssChannel(reader, this.rssItemRepository);
            channelBO.Name = channelDTO.Name;

            long userId = Helper.GetCurrentUserID(User);

            try
            {
                channelBO.Refresh();
                this.rssChannelRepository.AddByUserId(channelBO, userId);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                throw new HttpResponseException(message);
            }
        }

        // PUT api/RssChannels/5
        public void Put(long id, [FromBody]RssChannelDTO channelDTO)
        {
            // Update existing channel
            if (channelDTO == null)
                return;

            long userId = Helper.GetCurrentUserID(User);
            this.rssChannelRepository.UpdateByUserId(channelDTO, userId);
        }

        // DELETE api/RssChannels/5
        public void Delete(int id)
        {
            long userId = Helper.GetCurrentUserID(User);
            this.rssChannelRepository.DeleteByUserId(id, userId);
        }

        [HttpPut]
        public void Put(string action, long id)
        {
            if (action == null || id == 0)
            {
                string errorMessage = "Action is not defined or ID is equal to 0";
                log.Error(errorMessage);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(errorMessage) };
                throw new HttpResponseException(message);
            }

            switch (action)
            {
                case "update":
                    IRssChannel channelBO = this.rssChannelRepository.Get(id);
                    try
                    {
                        channelBO.Refresh();
                        this.rssChannelRepository.Save(channelBO);
                    }
                    catch (RssManager.Objects.Exceptions.RssSourceReadException ex)
                    {
                        log.Error(ex.ToString());
                        var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                        throw new HttpResponseException(message);
                    }
                    catch (RssManager.Objects.Exceptions.RssSourceParseException ex)
                    {
                        log.Error(ex.ToString());
                        var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                        throw new HttpResponseException(message);
                    }
                    break;
            }
        }
    }
}
