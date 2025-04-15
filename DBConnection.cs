using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace OnlearnEducation
{
    internal class DBConnection
    {
        private static string connectionString = "server=localhost;user=root;password=;database=onlearndb;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
