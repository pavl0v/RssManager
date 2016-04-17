using log4net;
using RssManager.Interfaces;
using RssManager.Interfaces.DTO;
using RssManager.Objects;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RssManager.WebAPI.Controllers
{
    public class RssItemsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssItemsController));
        private RssManager.Interfaces.Repository.IRssItemRepository rssItemRepository = null;

        public RssItemsController(RssManager.Interfaces.Repository.IRssItemRepository rssItemRepository)
        {
            this.rssItemRepository = rssItemRepository;
        }

        // GET api/rssitems
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/rssitems/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/rssitems
        //public void Post([FromBody]string value)
        //{
        //    // Create new item
        //}

        // PUT api/rssitems/5
        public void Put(int id, [FromBody]RssItem item)
        {
            // Update existing item
            
            if (item != null)
            {
                try
                {
                    this.rssItemRepository.Save(item);
                }
                catch (Exception ex)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(ex.Message),
                    };
                    throw new HttpResponseException(message);
                }
            }
        }

        // DELETE api/rssitems/5
        //public void Delete(int id)
        //{
        //}

        [HttpGet]
        public List<IRssItemDTO> Get(long channelid, int pageno, int pagesize)
        {
            long userId = Helper.GetCurrentUserID(User);
            List<IRssItemDTO> itemsDTO = this.rssItemRepository.GetByChannelId(channelid, pageno, pagesize, userId) as List<IRssItemDTO>;
            return itemsDTO;
        }

        [HttpPut]
        public void Put(string action, long id)
        {
            if (action == null || id == 0)
            {
                string errorMessage = "Action is not defined or ID is equal to 0.";
                log.Error(errorMessage);
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(errorMessage),
                };
                throw new HttpResponseException(message);
            }

            long userId = Helper.GetCurrentUserID(User);

            switch (action)
            {
                case "read":
                    this.rssItemRepository.SetReadState(id, userId, Interfaces.Enum.ReadState.IsRead);
                    break;

                case "readall":
                    //int rowsUpdated = RssManager.Dal.RssItemService.RI_MARK_RSSITEMS_AS_READ(id);
                    break;
            }
        }
    }
}
