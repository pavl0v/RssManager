using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using System;

namespace RssManager.Objects.BO
{
    public class User : IUserDTO, IEntity
    {
        #region FACTORY METHODS

        //public static User Create(string userName)
        //{
        //    UserDTO dto = UserService.USR_SELECT_USER_BY_USER_NAME(userName);
        //    return Create(dto);
        //}

        //public static User Create(UserDTO dto)
        //{
        //    if (dto == null)
        //        return null;

        //    return new User(dto);
        //}

        #endregion

        #region FIELDS

        private long id = 0;
        private string userName = string.Empty;
        private string password = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string guid = string.Empty;

        #endregion

        #region PROPERTIES

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public User(UserDTO dto)
        {
            this.firstName = dto.FirstName;
            this.id = dto.Id;
            this.lastName = dto.LastName;
            this.password = dto.Password;
            this.userName = dto.UserName;
            this.guid = dto.Guid;
        }

        #endregion
    }
}
