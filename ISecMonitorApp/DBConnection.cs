using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISecMonitorApp
{
    public static class DBConnection
    {
        public static MySqlConnection GetConnection()
        {
            string connectionString = "server=127.0.0.1;port=3306;database=isecmonitor;uid=root;pwd=;";
            return new MySqlConnection(connectionString);
        }
    }
}