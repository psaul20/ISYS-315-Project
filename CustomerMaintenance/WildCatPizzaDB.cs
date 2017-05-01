using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CustomerMaintenance
{
    /// <summary>
    /// This class contains connection information for the
    /// WildCatPizza database.
    /// </summary>
    public static class WildCatPizzaDB
    {
        /// <summary>
        /// This method returns a connection to allow access
        /// to an underlying database.
        /// </summary>
        /// <returns>MySQLConnection object sqlWildcatConnect
        /// which contains the database access information needed
        /// to open a connection to the database.</returns>
        public static MySqlConnection GetConnection()
        {
            string strConnectString =
                @"server=localhost;uid=root;database=wildcat_pizza;pwd=agemgiggies";
                MySqlConnection sqlWildcatConnect = new MySqlConnection(strConnectString);
            return sqlWildcatConnect;
        }
    }
}
