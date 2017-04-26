using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CustomerMaintenance
{
    public static class CustomerDB
    {
        public static Customer GetCustomer(int CustomerID)
        {
            MySqlConnection sqlWildcatConnect = WildCatPizzaDB.GetConnection();

            string strSelectStatement
                = "SELECT CUSTOMER_ID, CUSTOMER_LAST_NAME, CUSTOMER_FIRST_NAME, " 
                + "CUSTOMER_STREET_NUMBER, CUSTOMER_STREET_NAME, CUSTOMER_CITY, "
                + "CUSTOMER_STATE, CUSTOMER_PHONE, CUSTOMER_REFERRED_BY "
                + "FROM Customer "
                + "WHERE CUSTOMER_ID = @CustomerID";
            MySqlCommand sqlSelectCommand =
                new MySqlCommand(strSelectStatement, sqlWildcatConnect);
            sqlSelectCommand.Parameters.AddWithValue("@CustomerID", CustomerID);
            try
            {
                sqlWildcatConnect.Open();
                MySqlDataReader sqlCustReader =
                    sqlSelectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (sqlCustReader.Read())
                {
                    //string strReaderTest = "";
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    strReaderTest = strReaderTest + sqlCustReader.GetString(i);          
                    //}
                    //MessageBox.Show(strReaderTest);

                    Customer cstGetCust = new Customer();
                    cstGetCust.CustID = (int)sqlCustReader["CUSTOMER_ID"];
                    cstGetCust.CustLastName = sqlCustReader["CUSTOMER_LAST_NAME"].ToString();
                    cstGetCust.CustFirstName = sqlCustReader["CUSTOMER_FIRST_NAME"].ToString();
                    cstGetCust.CustStreetNum = (int)sqlCustReader["CUSTOMER_STREET_NUMBER"];
                    cstGetCust.CustStreetName = sqlCustReader["CUSTOMER_STREET_NAME"].ToString();
                    cstGetCust.CustCity = sqlCustReader["CUSTOMER_CITY"].ToString();
                    cstGetCust.CustState = sqlCustReader["CUSTOMER_STATE"].ToString();
                    cstGetCust.CustPhone = sqlCustReader["CUSTOMER_PHONE"].ToString();
                    //is there a more adaptable way to do this?
                    if (!sqlCustReader.IsDBNull(8))
                    {
                        cstGetCust.CustReferredBy = (int)sqlCustReader["CUSTOMER_REFERRED_BY"];
                    }
                    sqlCustReader.Close();
                    return cstGetCust;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlWildcatConnect.Close();
            }
        }

        public static int AddCustomer(Customer Customer)
        {
            MySqlConnection sqlWildcatConnect = WildCatPizzaDB.GetConnection();

            string strSelectMaxID
                = "SELECT Max(CUSTOMER_ID) "
                + "FROM Customer";
            MySqlCommand sqlSelectMaxID =
                new MySqlCommand(strSelectMaxID, sqlWildcatConnect);

            string strInsertStatement =
                "INSERT Customer " +
                "(CUSTOMER_ID, CUSTOMER_LAST_NAME, CUSTOMER_FIRST_NAME, CUSTOMER_STREET_NUMBER, " +
                "CUSTOMER_STREET_NAME, CUSTOMER_CITY, CUSTOMER_STATE, CUSTOMER_PHONE, CUSTOMER_REFERRED_BY) " +
                "VALUES (@CUSTOMER_ID, @CUSTOMER_LAST_NAME, @CUSTOMER_FIRST_NAME, @CUSTOMER_STREET_NUMBER, " +
                "@CUSTOMER_STREET_NAME, @CUSTOMER_CITY, @CUSTOMER_STATE, @CUSTOMER_PHONE, @CUSTOMER_REFERRED_BY) ";
            MySqlCommand sqlInsertNewCust =
                new MySqlCommand(strInsertStatement, sqlWildcatConnect);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_LAST_NAME", Customer.CustLastName);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_FIRST_NAME", Customer.CustFirstName);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_STREET_NUMBER", Customer.CustStreetNum);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_STREET_NAME", Customer.CustStreetName);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_CITY", Customer.CustCity);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_STATE", Customer.CustState);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_PHONE", Customer.CustPhone);
            sqlInsertNewCust.Parameters.AddWithValue(
                "@CUSTOMER_REFERRED_BY", Customer.CustReferredBy);

            try
            {
                sqlWildcatConnect.Open();

                int intMaxID = (int)sqlSelectMaxID.ExecuteScalar();
                Customer.CustID = intMaxID + 1;

                sqlInsertNewCust.Parameters.AddWithValue(
                    "@CUSTOMER_ID", Customer.CustID);

                sqlInsertNewCust.ExecuteNonQuery();

                return Customer.CustID;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlWildcatConnect.Close();
            }
        }

        public static bool UpdateCustomer(Customer OldCustomer,
            Customer NewCustomer)
        {
            MySqlConnection sqlWildcatConnect = WildCatPizzaDB.GetConnection();

            string strUpdateCust =
                //do we need to be able to change a customer's ID or referral? no to id, see to referral
                "UPDATE Customer SET " +
                //"CUSTOMER_ID = @NewID, " +
                "CUSTOMER_LAST_NAME = @NewLastName, " +
                "CUSTOMER_FIRST_NAME = @NewFirstName, " +
                "CUSTOMER_STREET_NUMBER = @NewStreetNum, " +
                "CUSTOMER_STREET_NAME = @NewStreetName, " +
                "CUSTOMER_CITY = @NewCity, " +
                "CUSTOMER_STATE = @NewState, " +
                "CUSTOMER_PHONE = @NewPhone, " +
                "CUSTOMER_REFERRED_BY = @NewReferral " +
                "WHERE CUSTOMER_ID = @OldID " +
                "AND CUSTOMER_LAST_NAME = @OldLastName " +
                "AND CUSTOMER_FIRST_NAME = @OldFirstName " +
                "AND CUSTOMER_STREET_NUMBER = @OldStreetNum " +
                "AND CUSTOMER_STREET_NAME = @OldStreetName " +
                "AND CUSTOMER_CITY = @OldCity " +
                "AND CUSTOMER_STATE = @OldState " +
                "AND CUSTOMER_PHONE = @OldPhone " +
                "AND CUSTOMER_REFERRED_BY = @OldReferral ";
            MySqlCommand sqlUpdateCustCommand =
                new MySqlCommand(strUpdateCust, sqlWildcatConnect);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewID", NewCustomer.CustID);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewLastName", NewCustomer.CustLastName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewFirstName", NewCustomer.CustFirstName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewStreetNum", NewCustomer.CustStreetNum);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewStreetName", NewCustomer.CustStreetName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewCity", NewCustomer.CustCity);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewState", NewCustomer.CustState);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewPhone", NewCustomer.CustPhone);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@NewReferral", NewCustomer.CustReferredBy);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldID", OldCustomer.CustID);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldLastName", OldCustomer.CustLastName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldFirstName", OldCustomer.CustFirstName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldStreetNum", OldCustomer.CustStreetNum);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldStreetName", OldCustomer.CustStreetName);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldCity", OldCustomer.CustCity);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldState", OldCustomer.CustState);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldPhone", OldCustomer.CustPhone);
            sqlUpdateCustCommand.Parameters.AddWithValue(
                "@OldReferral", OldCustomer.CustReferredBy);

            try
            {
                sqlWildcatConnect.Open();
                int intRowsAffected = Convert.ToInt32(sqlUpdateCustCommand.ExecuteNonQuery());
                if (intRowsAffected > 0)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
//MessageBox.Show(ex.Message, ex.GetType().ToString());
                throw ex;
            }
            finally
            {
                sqlWildcatConnect.Close();
            }
        }
    }
}
