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
    public class RssChannelRepository : IRssChannelRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssChannelRepository));
        private readonly IRssItemRepository rssItemRepository = null;

        public RssChannelRepository(IRssItemRepository rssItemRepository)
        {
            this.rssItemRepository = rssItemRepository;
        }

        #region IRssChannelRepository implementation

        public IEnumerable<IRssChannelDTO> GetByUserId(long userId)
        {
            List<IRssChannelDTO> lst = new List<IRssChannelDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RC_SELECT_RSSCHANNELS_BY_USER_ID(userId))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IRssChannelDTO dto = ReadRssChannel(reader);
                            lst.Add(dto);
                        }
                    }
                }
            }

            return lst;
        }

        public IEnumerable<IRssChannel> GetAll()
        {
            List<IRssChannel> lst = new List<IRssChannel>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RC_SELECT_RSSCHANNELS())
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IRssChannelDTO dto = ReadRssChannel(reader);
                            IRssChannel bo = new RssChannel(dto, this.rssItemRepository);
                            lst.Add(bo);
                        }
                    }
                }
            }

            return lst;
        }

        public void Add(IRssChannel entity)
        {
            throw new NotImplementedException();
        }

        public void AddByUserId(IRssChannel entity, long userId)
        {
            string procedureName = "RC_INSERT_RSSCHANNEL";
            string transactionName = string.Format("TR_{0}", procedureName);
            object obj = null;

            using (SqlConnection con = new SqlConnection(Common.ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(transactionName);

                SqlCommand cmd = SQLCMD_RC_INSERT_RSSCHANNEL(entity, userId);

                cmd.Connection = con;
                cmd.Transaction = tr;

                try
                {
                    obj = cmd.ExecuteScalar();
                    long channelId = obj == null ? 0 : Convert.ToInt64(obj);
                    entity.Id = channelId;

                    foreach (var i in entity.Items)
                    {
                        i.ChannelId = channelId;
                        SqlCommand c = RssItemRepository.SQLCMD_RI_INSERT_RSSITEM(i);
                        c.Connection = con;
                        c.Transaction = tr;
                        obj = c.ExecuteScalar();
                        long itemId = obj == null ? 0 : Convert.ToInt64(obj);
                        i.Id = itemId;
                    }

                    tr.Commit();
                    con.Close();
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("SQL command execution failed: {0}. Exception message: {1}", procedureName, ex.Message));

                    try
                    {
                        tr.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        log.Error(string.Format("Transaction rollback failed: {0}. Exception message: {1}", transactionName, ex2.Message));
                    }

                    throw;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        public void UpdateByUserId(IRssChannelDTO entity, long userId)
        {
            SqlCommand cmd = SQLCMD_RC_UPDATE_RSSCHANNEL_BY_USER_ID(entity, userId);
            int rowsUpdated = Common.ExecuteNonQuery(cmd, "RC_UPDATE_RSSCHANNEL_BY_USER_ID");
        }

        public int Delete(long id)
        {
            //SqlCommand cmd = SQLCMD_RC_DELETE_RSSCHANNEL(id);
            //return Common.ExecuteNonQuery(cmd, "DELETE_RSSCHANNEL");
            throw new NotImplementedException();
        }

        public int DeleteByUserId(long channelId, long userId)
        {
            SqlCommand cmd = SQLCMD_RC_DELETE_RSSCHANNEL_BY_USER_ID(channelId, userId);
            return Common.ExecuteNonQuery(cmd, "RC_DELETE_RSSCHANNEL_BY_USER_ID");
        }

        public void Update(IRssChannel entity)
        {
            SqlCommand cmd = SQLCMD_RC_UPDATE_RSSCHANNEL(entity);
            int rowsUpdated = Common.ExecuteNonQuery(cmd, "RC_UPDATE_RSSCHANNEL");
        }

        public void Save(IRssChannel entity)
        {
            if (entity.Id == 0)
            {
                try
                {
                    this.Add(entity);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Number == 2601)
                        throw new RssManager.Objects.Exceptions.DuplicateEntityException(entity.Url, "Unable to post duplicate RSS channel", ex);
                    throw;
                }

                return;
            }

            foreach (IRssItemDTO item in entity.Items)
            {
                this.rssItemRepository.Save(item);
            }
        }

        public IRssChannel Get(long id)
        {
            List<IRssChannel> lst = new List<IRssChannel>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RC_SELECT_RSSCHANNEL_BY_ID(id))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IRssChannelDTO dto = ReadRssChannel(reader);
                            IRssChannel bo = new RssChannel(dto, this.rssItemRepository);
                            lst.Add(bo);
                        }
                    }
                }
            }

            if (lst.Count > 0)
                return lst[0];

            string errorMsg = string.Format("Channel ID={0} does not exist", id);
            log.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        public IRssChannel Get(long channelId, long userId)
        {
            throw new NotImplementedException();
        }

        public IRssChannelDTO GetDTO(long channelId, long userId)
        {
            List<IRssChannelDTO> lst = new List<IRssChannelDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RC_SELECT_RSSCHANNEL_BY_ID_USER_ID(channelId, userId))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IRssChannelDTO dto = ReadRssChannel(reader);
                            lst.Add(dto);
                        }
                    }
                }
            }

            if (lst.Count > 0)
                return lst[0];

            string errorMsg = string.Format("Channel ID={0} does not exist", channelId);
            log.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        #endregion

        #region Private methods

        #region SQL commands

        private static SqlCommand SQLCMD_RC_INSERT_RSSCHANNEL(IRssChannel channel, long userId)
        {
            SqlCommand cmd = new SqlCommand("RC_INSERT_RSSCHANNEL");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@autorefresh", channel.Autorefresh));
            cmd.Parameters.Add(new SqlParameter("@copyright", channel.RssCopyright));
            cmd.Parameters.Add(new SqlParameter("@description", channel.RssDescription));
            cmd.Parameters.Add(new SqlParameter("@language", channel.RssLanguage));
            cmd.Parameters.Add(new SqlParameter("@name", channel.Name));
            cmd.Parameters.Add(new SqlParameter("@title", channel.RssTitle));
            cmd.Parameters.Add(new SqlParameter("@url", channel.Url));
            cmd.Parameters.Add(new SqlParameter("@user_id", userId));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_UPDATE_RSSCHANNEL(IRssChannelDTO channel)
        {
            SqlCommand cmd = new SqlCommand("RC_UPDATE_RSSCHANNEL");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@id", channel.Id));
            cmd.Parameters.Add(new SqlParameter("@copyright", channel.RssCopyright));
            cmd.Parameters.Add(new SqlParameter("@description", channel.RssDescription));
            cmd.Parameters.Add(new SqlParameter("@language", channel.RssLanguage));
            cmd.Parameters.Add(new SqlParameter("@name", channel.Name));
            cmd.Parameters.Add(new SqlParameter("@title", channel.RssTitle));
            cmd.Parameters.Add(new SqlParameter("@url", channel.Url));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_UPDATE_RSSCHANNEL_BY_USER_ID(IRssChannelDTO channel, long userId)
        {
            SqlCommand cmd = new SqlCommand("RC_UPDATE_RSSCHANNEL_BY_USER_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@channel_id", channel.Id));
            cmd.Parameters.Add(new SqlParameter("@user_id", userId));
            cmd.Parameters.Add(new SqlParameter("@name", channel.Name));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_DELETE_RSSCHANNEL(long channelId)
        {
            SqlCommand cmd = new SqlCommand("RC_DELETE_RSSCHANNEL");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@id", channelId));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_DELETE_RSSCHANNEL_BY_USER_ID(long channelId, long userId)
        {
            SqlCommand cmd = new SqlCommand("RC_DELETE_RSSCHANNEL_BY_USER_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@channel_id", channelId));
            cmd.Parameters.Add(new SqlParameter("@user_id", userId));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_SELECT_RSSCHANNEL_BY_ID(long channelId)
        {
            SqlCommand cmd = new SqlCommand("RC_SELECT_RSSCHANNEL_BY_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@id", channelId));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_SELECT_RSSCHANNELS()
        {
            SqlCommand cmd = new SqlCommand("RC_SELECT_RSSCHANNELS");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_SELECT_RSSCHANNEL_BY_ID_USER_ID(long channelId, long userId)
        {
            SqlCommand cmd = new SqlCommand("RC_SELECT_RSSCHANNEL_BY_ID_USER_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@channel_id", channelId));
            cmd.Parameters.Add(new SqlParameter("@user_id", userId));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_SELECT_RSSCHANNEL_BY_URL(string uri)
        {
            SqlCommand cmd = new SqlCommand("RC_SELECT_RSSCHANNEL_BY_URL");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@url", uri));

            return cmd;
        }

        private static SqlCommand SQLCMD_RC_SELECT_RSSCHANNELS_BY_USER_ID(long userId)
        {
            SqlCommand cmd = new SqlCommand("RC_SELECT_RSSCHANNELS_BY_USER_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@user_id", userId));

            return cmd;
        }

        #endregion

        private static RssChannelDTO ReadRssChannel(SqlDataReader reader)
        {
            RssChannelDTO dto = new RssChannelDTO();

            dto.Autorefresh = Convert.ToBoolean(reader[Common.CN_AUTOREFRESH]);
            dto.RssCopyright = Convert.ToString(reader[Common.CN_COPYRIGHT]);
            dto.RssDescription = Convert.ToString(reader[Common.CN_DESCRIPTION]);
            dto.Id = Convert.ToInt64(reader[Common.CN_ID]);
            dto.Name = Convert.ToString(reader[Common.CN_NAME]);
            dto.RssLanguage = Convert.ToString(reader[Common.CN_LANGUAGE]);
            dto.RssTitle = Convert.ToString(reader[Common.CN_TITLE]);
            dto.Url = Convert.ToString(reader[Common.CN_URL]);

            return dto;
        }

        #endregion
    }
}
