using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CustomerMaintenance
{
    /// <summary>
    /// This class handles all connections and SQL
    /// statements passed along to the Customer table
    /// within the Wildcat Pizza database.
    /// </summary>
    public static class CustomerDB
    {
        /// <summary>
        /// This method receives a Customer ID as an input,
        /// writes an SQL statement, passes that statement along
        /// to the database in order to retrieve the information 
        /// associated with that CustomerID, then loads the 
        /// information into a Customer object and returns it.
        /// </summary>
        /// <param name="CustomerID"> The customer ID associated
        /// with the customer that the user seeks to retrieve.</param>
        /// <returns>cstGetCust, a customer object that contains
        /// properties corresponding to that customer's fields in the 
        /// database.</returns>
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
                    Customer cstGetCust = new Customer();
                    cstGetCust.CustID = (int)sqlCustReader["CUSTOMER_ID"];
                    cstGetCust.CustLastName = sqlCustReader["CUSTOMER_LAST_NAME"].ToString();
                    cstGetCust.CustFirstName = sqlCustReader["CUSTOMER_FIRST_NAME"].ToString();
                    cstGetCust.CustStreetNum = (int)sqlCustReader["CUSTOMER_STREET_NUMBER"];
                    cstGetCust.CustStreetName = sqlCustReader["CUSTOMER_STREET_NAME"].ToString();
                    cstGetCust.CustCity = sqlCustReader["CUSTOMER_CITY"].ToString();
                    cstGetCust.CustState = sqlCustReader["CUSTOMER_STATE"].ToString();
                    cstGetCust.CustPhone = sqlCustReader["CUSTOMER_PHONE"].ToString();

                    //This if statement checks whether or not the customer has a
                    //referral number associated with it in the database, then writes
                    //that referral number to the CustReferredBy property or omits it
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
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlWildcatConnect.Close();
            }
        }

        /// <summary>
        /// This method receives a customer object from an
        /// Add customer form, queries the database to receive the 
        /// current highest customer ID, sets the new customer's ID to
        /// 1 + the highest ID number, then writes a sql statement to insert
        /// the customer's properties into the database and passes it along
        /// to the database.
        /// </summary>
        /// <param name="Customer"> The customer object that contains the
        /// properties that the user seeks to add to the database.</param>
        /// <returns>The Customer ID associated with the customer that
        /// was successfully added to the database.</returns>
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

            //This if/else statement checks whether or not the added
            //customer has a referral number associated with it, then 
            //passes inserts the referral number or a null value into
            //the statement depending on whether or not a referral is
            //present.
            if (Customer.CustReferredBy != 0)
                sqlInsertNewCust.Parameters.AddWithValue(
                    "@CUSTOMER_REFERRED_BY", Customer.CustReferredBy);
            else
                sqlInsertNewCust.Parameters.AddWithValue(
                    "@CUSTOMER_REFERRED_BY", null);

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

        /// <summary>
        /// This method receives 2 customer objects, one representing the 
        /// customer's properties pre-modification, and one representing the
        /// customer's properties post-modification. The method then writes an
        /// SQL Update statement to make the appropriate changes to the customer
        /// in the database, passes it along to the database, and returns the
        /// number of rows affected to confirm that the changes were made 
        /// successfully.
        /// </summary>
        /// <param name="OldCustomer">A customer object that contains the properties
        /// of a customer pre-modification</param>
        /// <param name="NewCustomer">A customer object that contains the properties
        /// of a customer post-modification</param>
        /// <returns>intRowsaffected which represents the number of rows affected 
        /// by the SQL Update.</returns>
        public static bool UpdateCustomer(Customer OldCustomer,
            Customer NewCustomer)
        {
            MySqlConnection sqlWildcatConnect = WildCatPizzaDB.GetConnection();

            string strUpdateCust =
                "UPDATE Customer SET " +
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
                "AND CUSTOMER_PHONE = @OldPhone ";
            if (OldCustomer.CustReferredBy != 0)
                strUpdateCust = 
                    strUpdateCust + "AND CUSTOMER_REFERRED_BY = @OldReferral ";

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
            if (NewCustomer.CustReferredBy != 0)
                sqlUpdateCustCommand.Parameters.AddWithValue(
                    "@NewReferral", NewCustomer.CustReferredBy);
            else
                sqlUpdateCustCommand.Parameters.AddWithValue(
                    "@NewReferral", null);
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
            if (OldCustomer.CustReferredBy != 0)
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
                throw ex;
            }
            finally
            {
                sqlWildcatConnect.Close();
            }
        }
    }
}
