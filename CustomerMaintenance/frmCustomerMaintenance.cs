//AUTHOR:		Fantastic Four 
//              Hannah Christman, David Cisarik
//              Grace Evans, Patrick Saul
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
//ADDITIONAL
//DETAILS:      This program assumes that the underlying database contains null
//              wherever a customer does not have a referring customer associated
//              with it.
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
        /// This event handler for the Get Customer button
        /// calls methods from the validator class which 
        /// validate the data inputted into the "Customer ID"
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

        /// <summary>
        /// This method receives the customer ID that was passed along
        /// by the btnGetCustomer_Click event handler, and then passes
        /// the ID along to the GetCustomer method in the CustomerDB
        /// class to retrieve a customer from the database. Once the
        /// customer details are retrieved, they are assigned to the
        /// properties of the previously instantiated cstCustomer object.
        /// TryCatch is included to handle any exceptions that are thrown
        /// when communicating with the database.
        /// </summary>
        /// <param name="CustomerID">The Customer ID associated with
        /// the customer that the user seeks to retrieve details for.</param>
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

        /// <summary>
        /// This method clears the text boxes when a Customer ID is
        /// inputted into the CustomerID box, but the corresponding
        /// customer information cannot be found.
        /// </summary>
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

        /// <summary>
        /// This method loads the properties of the instantiated
        /// cstCustomer object into the textboxes when a customer
        /// is retrieved from the database.
        /// </summary>
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

        /// <summary>
        /// This event handler for the Add button
        /// instantiates a new frmAddModifyCustomer
        /// as an add customer form, presents the form as a dialog box,
        /// then waits for the result of the user's interaction with the
        /// new form. If the user succesfully adds a new customer into the database
        /// the form stores the newly added customer into the cstCustomer object
        /// and displays the customer's details on the original form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// This event handler for the Modify button
        /// instantiates a new frmAddModifyCustomer as
        /// a modify customer form, passes the previously instantiated 
        /// and defined cstCustomer object onto the modify customer form,
        /// presents the form as a dialog box, then waits for the result.
        /// if the customer successfully modifies the customer, the newly
        /// modified customer's details will be displayed in the read-only
        /// textboxes on the original form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            frmAddModifyCustomer frmModifyCustomer = new frmAddModifyCustomer();
            frmModifyCustomer.blnAddCustomer = false;
            frmModifyCustomer.cstAddModCust = cstCustomer;
            DialogResult result = frmModifyCustomer.ShowDialog();
            if (result == DialogResult.OK)
            {
                cstCustomer = frmModifyCustomer.cstAddModCust;
                DisplayCustomer();
            }
            else if (result == DialogResult.Retry)
            {
                GetCustomer(cstCustomer.CustID);
                if (cstCustomer != null)
                    DisplayCustomer();
                else
                    ClearControls();
            }
        }

        /// <summary>
        /// This event handler closes the form when the exit button
        /// is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
