using System;

namespace RssManager.Interfaces.DTO
{
    public class SubscriberDTO : ISubscriberDTO
    {
        public string ChannelName { get; set; }
        public string FirstName { get; set; }
        public long Id { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Guid { get; set; }
    }
}
