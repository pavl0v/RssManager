using RssManager.Interfaces.BO;
using System;

namespace RssManager.Interfaces.DTO
{
    public interface IUserDTO : IEntity
    {
        string FirstName { get; set; }
        long Id { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
        string Guid { get; set; }
    }
}
