using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class Database
    {
        public Database()
        {
            List<string> results = new List<string>();
            SqlConnection conn = new SqlConnection("Data Source = 46.30.212.82; Initial Catalog = furyzone_com; User Id = furyzone_com; Password = gzLxJ8ga;");
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = "insert into db_blc_global values (NULL, 'ptx', 'says hello', NULL);";
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(dr["myColumn"].ToString());
                    }
                }
            }
        }
    }
}
