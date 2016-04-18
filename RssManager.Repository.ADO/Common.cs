using log4net;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RssManager.Repository.ADO
{
    internal static class Common
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Common));

        #region COLUMN NAMES

        public const string CN_AUTHOR = "AUTHOR";
        public const string CN_CATEGORY = "CATEGORY";
        public const string CN_CHANNEL_ID = "CHANNEL_ID";
        public const string CN_COMMENTS = "COMMENTS";
        public const string CN_COPYRIGHT = "COPYRIGHT";
        public const string CN_DESCRIPTION = "DESCRIPTION";
        public const string CN_ENCLOSURE = "ENCLOSURE";
        public const string CN_GUID = "GUID";
        public const string CN_ID = "ID";
        public const string CN_LANGUAGE = "LANGUAGE";
        public const string CN_LINK = "LINK";
        public const string CN_NAME = "NAME";
        public const string CN_PUB_DATE = "PUB_DATE";
        public const string CN_PUB_DATE_TIME = "PUB_DATE_TIME";
        public const string CN_READ_STATE = "READ_STATE";
        public const string CN_SOURCE = "SOURCE";
        public const string CN_TITLE = "TITLE";
        public const string CN_URL = "URL";
        public const string CN_USER_ID = "USER_ID";
        public const string CN_FIRST_NAME = "FIRST_NAME";
        public const string CN_LAST_NAME = "LAST_NAME";
        public const string CN_USER_NAME = "USER_NAME";
        public const string CN_PASSWORD = "PASSWORD";
        public const string CN_RSSCHANNEL_NAME = "RSSCHANNEL_NAME";
        public const string CN_AUTOREFRESH = "AUTOREFRESH";

        #endregion

        #region PARAMETER NAMES

        public const string P_AUTHOR = "@author";
        public const string P_CATEGORY = "@category";
        public const string P_CHANNEL_ID = "@channel_id";
        public const string P_COMMENTS = "@comments";
        public const string P_DESCRIPTION = "@description";
        public const string P_ENCLOSURE = "@enclosure";
        public const string P_GUID = "@guid";
        public const string P_ID = "@id";
        public const string P_LINK = "@link";
        public const string P_PUB_DATE = "@pub_date";
        public const string P_PUB_DATE_TIME = "@pub_date_time";
        public const string P_READ_STATE = "@read_state";
        public const string P_SOURCE = "@source";
        public const string P_TITLE = "@title";
        public const string P_URL = "@url";

        #endregion

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[
                    ConfigurationManager.AppSettings["ActiveConnectionString"]].ConnectionString;
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static int ExecuteNonQuery(SqlCommand cmd, string name)
        {
            string procedureName = name;
            string transactionName = string.Format("TR_{0}", procedureName);
            if (transactionName.Length > 32)
                transactionName = transactionName.Substring(0, 31);
            int rowsAffected = 0;

            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(transactionName);

                cmd.Connection = con;
                cmd.Transaction = tr;

                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                    tr.Commit();
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

                con.Close();
            }

            return rowsAffected;
        }

        public static T ExecuteScalar<T>(SqlCommand cmd, string name)
        {
            string procedureName = name;
            string transactionName = string.Format("{0}_TR", procedureName);
            T res = default(T);

            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(transactionName);

                cmd.Connection = con;
                cmd.Transaction = tr;

                try
                {
                    object obj = cmd.ExecuteScalar();
                    if (obj is T)
                    {
                        res = (T)obj;
                    }
                    else
                    {
                        try
                        {
                            res = (T)Convert.ChangeType(obj, typeof(T));
                        }
                        catch (InvalidCastException)
                        {
                            throw;
                        }
                    }

                    tr.Commit();
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
                        throw;
                    }

                    throw;
                }

                con.Close();
            }

            return res;
        }
    }
}
