using log4net;
using RssManager.Interfaces.DTO;
using RssManager.Interfaces.Enum;
using RssManager.Interfaces.Repository;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RssManager.Repository.ADO
{
    public class RssItemRepository : IRssItemRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssItemRepository));

        #region IRssItemRepository implementation

        public IEnumerable<IRssItemDTO> GetByChannelId(long channelId)
        {
            List<IRssItemDTO> lst = new List<IRssItemDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID(channelId))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RssItemDTO dto = ReadRssItem(reader);
                            IRssItemDTO bo = new RssItem(dto);
                            lst.Add(bo);
                        }
                    }

                    con.Close();
                }
            }

            return lst;
        }

        public IEnumerable<IRssItemDTO> GetByChannelId(long channelId, int pageNo, int pageSize, long userId)
        {
            List<IRssItemDTO> lst = new List<IRssItemDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID_PAGE(channelId, pageNo, pageSize, userId))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RssItemDTO dto = ReadRssItem(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            return lst;
        }

        public IEnumerable<IRssItemDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Add(IRssItemDTO entity)
        {
            string procedureName = "RI_INSERT_RSSITEM";
            string transactionName = string.Format("{0}_TR", procedureName);
            object obj = null;

            using (SqlConnection con = new SqlConnection(Common.ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(transactionName);

                SqlCommand cmd = SQLCMD_RI_INSERT_RSSITEM(entity);

                cmd.Connection = con;
                cmd.Transaction = tr;

                try
                {
                    obj = cmd.ExecuteScalar();
                    long id = obj == null ? 0 : Convert.ToInt64(obj);
                    entity.Id = id;

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
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        public int Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(IRssItemDTO entity)
        {
            SqlCommand cmd = SQLCMD_RI_UPDATE_RSSITEM(entity);
            Common.ExecuteNonQuery(cmd, "UPDATE_RSSITEM");
        }

        public void Save(IRssItemDTO entity)
        {
            if (entity.Id == 0)
            {
                IRssItemDTO existingItem = this.GetByGuid(entity.RssGuid);
                if (existingItem != null)
                {
                    if (existingItem.PubDateTime < entity.PubDateTime)
                    {
                        entity.Id = existingItem.Id;
                        entity.ReadState = Interfaces.Enum.ReadState.IsNew;
                        this.Update(entity);
                    }
                }
                else
                {
                    //this.rssItemRepository.Add(item);
                    this.Add(entity);
                }
            }
            else
            {
                this.Update(entity);
            }
        }

        public IRssItemDTO Get(long id)
        {
            List<RssItemDTO> lst = new List<RssItemDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RI_SELECT_RSSITEM_BY_ID(id))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RssItemDTO dto = ReadRssItem(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            if (lst.Count > 0)
            {
                return new RssItem(lst[0]);
            }

            string errorMsg = string.Format("There is no channel: ID={0}", id);
            log.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        public int SetReadState(long itemId, long userId, ReadState state)
        {
            SqlCommand cmd = SQLCMD_RI_SET_READ_STATE(itemId, userId, state);
            return Common.ExecuteNonQuery(cmd, "SET_READ_STATE");
        }

        public IRssItemDTO GetByGuid(string guid)
        {
            List<RssItemDTO> lst = new List<RssItemDTO>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_RI_SELECT_RSSITEM_BY_GUID(guid))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RssItemDTO dto = ReadRssItem(reader);
                            lst.Add(dto);
                        }
                    }

                    con.Close();
                }
            }

            if (lst.Count > 0)
            {
                return new RssItem(lst[0]);
            }

            //string errorMsg = string.Format("There is no channel: GUID={0}", guid);
            //log.Error(errorMsg);
            //throw new Exception(errorMsg);
            return null;
        }

        #endregion

        #region Private methods

        #region SQL commands

        internal static SqlCommand SQLCMD_RI_INSERT_RSSITEM(IRssItemDTO item)
        {
            SqlCommand cmd = new SqlCommand("RI_INSERT_RSSITEM");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channel_id", item.ChannelId);
            cmd.Parameters.AddWithValue("@pub_date", item.RssPubDate);
            cmd.Parameters.AddWithValue("@pub_date_time", item.PubDateTime);
            cmd.Parameters.AddWithValue("@author", item.RssAuthor);
            cmd.Parameters.AddWithValue("@category", item.RssCategory);
            cmd.Parameters.AddWithValue("@comments", item.RssComments);
            cmd.Parameters.AddWithValue("@description", item.RssDescription);
            cmd.Parameters.AddWithValue("@enclosure", item.RssEnclosure);
            cmd.Parameters.AddWithValue("@guid", item.RssGuid);
            cmd.Parameters.AddWithValue("@link", item.RssLink);
            cmd.Parameters.AddWithValue("@source", item.RssSource);
            cmd.Parameters.AddWithValue("@title", item.RssTitle);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID(long channelId)
        {
            SqlCommand cmd = new SqlCommand("RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channel_id", channelId);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID_PAGE(long channelId, int pageNo, int pageSize, long userId)
        {
            SqlCommand cmd = new SqlCommand("RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID_PAGE");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channel_id", channelId);
            cmd.Parameters.AddWithValue("@page_no", pageNo);
            cmd.Parameters.AddWithValue("@page_size", pageSize);
            cmd.Parameters.AddWithValue("@user_id", userId);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_SELECT_RSSITEM_BY_ID(long id)
        {
            SqlCommand cmd = new SqlCommand("RI_SELECT_RSSITEM_BY_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_SELECT_RSSITEM_BY_GUID(string guid)
        {
            SqlCommand cmd = new SqlCommand("RI_SELECT_RSSITEM_BY_GUID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@guid", guid);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_UPDATE_RSSITEM(IRssItemDTO item)
        {
            SqlCommand cmd = new SqlCommand("RI_UPDATE_RSSITEM");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(Common.P_ID, item.Id);
            cmd.Parameters.AddWithValue(Common.P_CHANNEL_ID, item.ChannelId);
            cmd.Parameters.AddWithValue(Common.P_AUTHOR, item.RssAuthor);
            cmd.Parameters.AddWithValue(Common.P_CATEGORY, item.RssCategory);
            cmd.Parameters.AddWithValue(Common.P_COMMENTS, item.RssComments);
            cmd.Parameters.AddWithValue(Common.P_DESCRIPTION, item.RssDescription);
            cmd.Parameters.AddWithValue(Common.P_ENCLOSURE, item.RssEnclosure);
            cmd.Parameters.AddWithValue(Common.P_GUID, item.RssGuid);
            cmd.Parameters.AddWithValue(Common.P_LINK, item.RssLink);
            cmd.Parameters.AddWithValue(Common.P_PUB_DATE, item.RssPubDate);
            cmd.Parameters.AddWithValue(Common.P_PUB_DATE_TIME, item.PubDateTime);
            cmd.Parameters.AddWithValue(Common.P_SOURCE, item.RssSource);
            cmd.Parameters.AddWithValue(Common.P_TITLE, item.RssTitle);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_MARK_RSSITEMS_AS_READ(long channelId)
        {
            SqlCommand cmd = new SqlCommand("RI_MARK_RSSITEMS_AS_READ");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channelId", channelId);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_GET_RSSITEMS_COUNT_BY_RSSCHANNEL_ID(long channelId)
        {
            SqlCommand cmd = new SqlCommand("RI_GET_RSSITEMS_COUNT_BY_RSSCHANNEL_ID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@channel_id", channelId);

            return cmd;
        }

        private static SqlCommand SQLCMD_RI_SET_READ_STATE(long itemId, long userId, ReadState state)
        {
            SqlCommand cmd = new SqlCommand("RI_SET_READ_STATE");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@item_id", itemId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@state", (byte)state);

            return cmd;
        }

        #endregion

        private static RssItemDTO ReadRssItem(SqlDataReader reader)
        {
            RssItemDTO dto = new RssItemDTO(); 

            dto.RssAuthor = Convert.ToString(reader[Common.CN_AUTHOR]);
            dto.RssCategory = Convert.ToString(reader[Common.CN_CATEGORY]);
            dto.ChannelId = Convert.ToInt64(reader[Common.CN_CHANNEL_ID]);
            dto.RssComments = Convert.ToString(reader[Common.CN_COMMENTS]);
            dto.RssDescription = Convert.ToString(reader[Common.CN_DESCRIPTION]);
            dto.RssEnclosure = Convert.ToString(reader[Common.CN_ENCLOSURE]);
            dto.RssGuid = Convert.ToString(reader[Common.CN_GUID]);
            dto.Id = Convert.ToInt64(reader[Common.CN_ID]);
            dto.RssLink = Convert.ToString(reader[Common.CN_LINK]);
            dto.RssPubDate = Convert.ToString(reader[Common.CN_PUB_DATE]);
            dto.PubDateTime = Convert.ToDateTime(reader[Common.CN_PUB_DATE_TIME]);
            dto.RssSource = Convert.ToString(reader[Common.CN_SOURCE]);
            dto.RssTitle = Convert.ToString(reader[Common.CN_TITLE]);

            try
            {
                dto.ReadState = reader[Common.CN_READ_STATE] != DBNull.Value ? (ReadState)Convert.ToByte(reader[Common.CN_READ_STATE]) : ReadState.IsNew;
            }
            catch (IndexOutOfRangeException)
            {
                dto.ReadState = ReadState.IsNew;
            }

            return dto;
        }

        #endregion
    }
}
