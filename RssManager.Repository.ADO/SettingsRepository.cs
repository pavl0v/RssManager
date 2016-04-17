using RssManager.Interfaces.BO;
using RssManager.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RssManager.Repository.ADO
{
    public class SettingsRepository : ISettingsRepository
    {
        public ISettings Get()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (SqlConnection con = Common.GetConnection())
            {
                using (SqlCommand cmd = SQLCMD_SET_SELECT_SETTINGS())
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KeyValuePair<string, string> kvp = ReadSetting(reader);
                            dict.Add(kvp.Key, kvp.Value);
                        }
                    }

                    con.Close();
                }
            }

            Objects.BO.Settings bo = new Objects.BO.Settings(dict);
            return bo;
        }

        private static SqlCommand SQLCMD_SET_SELECT_SETTINGS()
        {
            SqlCommand cmd = new SqlCommand("SET_SELECT_SETTINGS");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return cmd;
        }

        private static KeyValuePair<string, string> ReadSetting(SqlDataReader reader)
        {
            string key = string.Empty;
            string val = string.Empty;
            key = Convert.ToString(reader["KEY"]);
            val = Convert.ToString(reader["VALUE"]);
            KeyValuePair<string, string> kvp = new KeyValuePair<string,string>(key, val);
            return kvp;
        }
    }
}
