//AUTHOR:		Your Name
//COURSE:		ISYS 315.501
//FORM:		    frmCustomerMaintenance
//PURPOSE:		This program is intended to allow users to add 
//              customers to an existing underlying customer database
//              or update customers that already exist.
//INITIALIZE:	N/A
//INPUT:		If a user seeks to pull the information from an existing
//              customer, the customer's ID should be input on the first
//              form. If a user seeks to add a customer, the user
//              hits the "add" button and then must input the customer
//              details they want to put in the database. If a user seeks
//              to modify an existing customer, the user inputs an existing
//              customer ID, then hits the "modify" button and inputs 
//              any changes to the existing customer details he/she desires.
//PROCESS:		If retrieving an existing customer, the GetCustomer
//              method is called which generates a new instance of
//              the "customer" class, queries the database with 
//              the given customer ID, loads the resulting customer
//              details into the properties of the instantiated customer,
//              which then allows the details to be passed along to the 
//              DisplayCustomer method to be outputted.
//              If a customer is being added or modified, the program takes the
//              customer details provided by the user, loads them into
//              a new instance of the customer class, passes the information
//              on to the database, then passes the instantiated customer
//              onto the DisplayCustomer method to be outputted.
//OUTPUT:		If an existing customer is retrieved from the database
//              using that customer's ID on the initial form, all of the
//              customer's details will be displayed in read-only text
//              boxes on the initial form. If a customer is added or
//              modified, the new/modified customer details will be displayed
//              on the initial form once the user is finished interacting
//              with the add/modify form.
//TERMINATE:	Because the program retrieves and pushes information from and to
//              a MySQL database, a connection is opened and then terminated once
//              the SQL query is passed along, and any pertinent results are handled.
//              Similarly, a MySQL data reader is opened to retrieve the results of
//              the GetCustomer query, and closed once the necessary information has
//              been handled.
//HONOR CODE:	“On my honor, as an Aggie, I have neither given 
//			    nor received unauthorized aid on this academic 
//			    work.”



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CustomerMaintenance
{
    public partial class frmCustomerMaintenance : Form
    {
        public frmCustomerMaintenance()
        {
            InitializeComponent();
        }

        //A customer is instantiated to house the details of any customer
        //sent to or retrieved from the database.
        private Customer cstCustomer;

        /// <summary>
        /// This method validates the data inputted into the "Customer ID"
        /// text box on the inital form for the program, passes that ID
        /// on to the GetCustomer method, then displays the details of 
        /// that customer once they are retrieved from the database and
        /// loaded into the instantiated cstCustomer object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCustomer_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtCustomerID) &&
                Validator.IsInt32(txtCustomerID))
            {
                int intCustomerID = Convert.ToInt32(txtCustomerID.Text);
                GetCustomer(intCustomerID);
                if (cstCustomer == null)
                {
                    MessageBox.Show("No customer found with this ID. " +
                         "Please try again.", "Customer Not Found");
                    ClearControls();
                }
                else
                    DisplayCustomer();
            }
        }


        private void GetCustomer(int CustomerID)
        {
            try
            {
                cstCustomer = CustomerDB.GetCustomer(CustomerID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        private void ClearControls()
        {
            txtCustomerID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtStreetNum.Text = "";
            txtStreetName.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPhone.Text = "";
            txtReferred.Text = "";
            btnModify.Enabled = false;
            txtCustomerID.Focus();
        }

        private void DisplayCustomer()
        {
            txtFirstName.Text = cstCustomer.CustFirstName;
            txtLastName.Text = cstCustomer.CustLastName;
            txtStreetNum.Text = cstCustomer.CustStreetNum.ToString();
            txtStreetName.Text = cstCustomer.CustStreetName;
            txtCity.Text = cstCustomer.CustCity;
            txtState.Text = cstCustomer.CustState;
            txtPhone.Text = cstCustomer.CustPhone;
            if (cstCustomer.CustReferredBy == 0)
            {
                txtReferred.Text = "N/A";
            }
            else
            {
                txtReferred.Text = cstCustomer.CustReferredBy.ToString();
            }
            btnModify.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddModifyCustomer frmAddCustomerForm = new frmAddModifyCustomer();
            frmAddCustomerForm.blnAddCustomer = true;
            DialogResult dlgResult = frmAddCustomerForm.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                cstCustomer = frmAddCustomerForm.cstAddModCust;
                txtCustomerID.Text = cstCustomer.CustID.ToString();
                DisplayCustomer();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            frmAddModifyCustomer frmModifyCustomer = new frmAddModifyCustomer();
            frmModifyCustomer.blnAddCustomer = false;
            frmModifyCustomer.cstAddModCust = cstCustomer;
            DialogResult result = frmModifyCustomer.ShowDialog();
            if (result == DialogResult.OK)
            {
                cstCustomer = frmModifyCustomer.cstAddModCust;
                this.DisplayCustomer();
            }
            else if (result == DialogResult.Retry)
            {
                this.GetCustomer(cstCustomer.CustID);
                if (cstCustomer != null)
                    this.DisplayCustomer();
                else
                    this.ClearControls();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
