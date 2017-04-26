using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace CustomerMaintenance
{
    public static class WildCatPizzaDB
    {
        public static MySqlConnection GetConnection()
        {
            string strConnectString =
                @"server=localhost;uid=root;database=wildcat_pizza;pwd=agemgiggies";
                MySqlConnection sqlWildcatConnect = new MySqlConnection(strConnectString);
            return sqlWildcatConnect;
        }
    }
}
