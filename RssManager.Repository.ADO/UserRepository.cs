using log4net;
using RssManager.Interfaces.BO;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Repository;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RssManager.Repository.ADO
{
    public class UserRepository : IUserRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepository));

        #region IUserRepository implementation

        public IUserDTO GetByUsername(string username)
        {
            List<UserDTO> lst = new List<UserDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_USR_SELECT_USER_BY_USER_NAME(username))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDTO dto = ReadUser(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            if (lst.Count > 0)
            {
                return new User(lst[0]);
            }

            string errorMsg = string.Format("Account '{0}' does not exist in database", username);
            log.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        public List<ISubscriberDTO> GetByRssChannelId(long channelId)
        {
            List<ISubscriberDTO> lst = new List<ISubscriberDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_USR_SELECT_USERS_BY_RSSCHANNEL_ID(channelId))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ISubscriberDTO dto = ReadSubscriber(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            return lst;
        }

        public IEnumerable<IUserDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Add(IUserDTO entity)
        {
            long id = 0;

            using (SqlCommand cmd = SQLCMD_USR_INSERT_USER(entity))
            {
                id = Common.ExecuteScalar<long>(cmd, "INSERT_USER");
                entity.Id = id;
            }
        }

        public int Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(IUserDTO entity)
        {
            SqlCommand cmd = SQLCMD_USR_UPDATE_USER(entity);
            Common.ExecuteNonQuery(cmd, "USR_UPDATE_USER");
        }

        public void Save(IUserDTO entity)
        {
            if (entity.Id == 0)
                this.Add(entity);
            else
                this.Update(entity);
        }

        public IUserDTO Get(long id)
        {
            List<UserDTO> lst = new List<UserDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_USR_SELECT_USER_BY_ID(id))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDTO dto = ReadUser(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            if (lst.Count > 0)
            {
                return new User(lst[0]);
            }

            string errorMsg = string.Format("Account ID={0} does not exist in database", id);
            log.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        #endregion

        #region Private methods

        #region SQL commands

        private static SqlCommand SQLCMD_USR_SELECT_USERS_BY_RSSCHANNEL_ID(long channelId)
        {
            SqlCommand cmd = new SqlCommand("USR_SELECT_USERS_BY_RSSCHANNEL_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channel_id", channelId);

            return cmd;
        }

        private static SqlCommand SQLCMD_USR_SELECT_USER_BY_USER_NAME(string username)
        {
            SqlCommand cmd = new SqlCommand("USR_SELECT_USER_BY_USER_NAME");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            return cmd;
        }

        private static SqlCommand SQLCMD_USR_SELECT_USER_BY_ID(long id)
        {
            SqlCommand cmd = new SqlCommand("USR_SELECT_USER_BY_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            return cmd;
        }

        private static SqlCommand SQLCMD_USR_INSERT_USER(IUserDTO user)
        {
            SqlCommand cmd = new SqlCommand("USR_INSERT_USER");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@first_name", user.FirstName);
            cmd.Parameters.AddWithValue("@last_name", user.LastName);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@user_name", user.UserName);
            cmd.Parameters.AddWithValue("@guid", user.Guid);

            return cmd;
        }

        private static SqlCommand SQLCMD_USR_UPDATE_USER(IUserDTO user)
        {
            SqlCommand cmd = new SqlCommand("USR_UPDATE_USER");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@first_name", user.FirstName);
            cmd.Parameters.AddWithValue("@last_name", user.LastName);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@user_name", user.UserName);

            return cmd;
        }

        #endregion

        private static UserDTO ReadUser(SqlDataReader reader)
        {
            UserDTO dto = new UserDTO();

            dto.FirstName = Convert.ToString(reader[Common.CN_FIRST_NAME]);
            dto.Id = Convert.ToInt64(reader[Common.CN_ID]);
            dto.LastName = Convert.ToString(reader[Common.CN_LAST_NAME]);
            dto.Password = Convert.ToString(reader[Common.CN_PASSWORD]);
            dto.UserName = Convert.ToString(reader[Common.CN_USER_NAME]);
            dto.Guid = Convert.ToString(reader[Common.CN_GUID]);

            return dto;
        }

        private static SubscriberDTO ReadSubscriber(SqlDataReader reader)
        {
            SubscriberDTO dto = new SubscriberDTO();

            dto.FirstName = Convert.ToString(reader[Common.CN_FIRST_NAME]);
            dto.Id = Convert.ToInt64(reader[Common.CN_ID]);
            dto.LastName = Convert.ToString(reader[Common.CN_LAST_NAME]);
            dto.Password = Convert.ToString(reader[Common.CN_PASSWORD]);
            dto.UserName = Convert.ToString(reader[Common.CN_USER_NAME]);
            dto.Guid = Convert.ToString(reader[Common.CN_GUID]);
            dto.ChannelName = Convert.ToString(reader[Common.CN_RSSCHANNEL_NAME]);

            return dto;
        }

        #endregion
    }
}
