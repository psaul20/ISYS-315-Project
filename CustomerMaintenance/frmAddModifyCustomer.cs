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
    public partial class frmAddModifyCustomer : Form
    {
        public frmAddModifyCustomer()
        {
            InitializeComponent();
        }

        //This boolean variable changes the behavior of the form
        //based on whether the user wants to add or modify a customer
        public bool blnAddCustomer;

        //This customer definition will house details for a
        //customer that will be added into the database or an existing
        //customer that will be modified
        public Customer cstAddModCust;

        /// <summary>
        /// This event handler sets up the initial display of
        /// the AddModifyCustomer form based on the blnAddCustomer
        /// variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAddModifyCustomer_Load(object sender, EventArgs e)
        {
            if (blnAddCustomer)
            {
                Text = "Add Customer";
                cboStates.SelectedIndex = -1;
            }
            else
            {
                Text = "Modify Customer";
                DisplayCustomer();
            }
        }

        /// <summary>
        /// This method displays an existing customer's information
        /// into the text boxes on a Modify Customer form to allow
        /// the user to make changes to the customer's existing
        /// information.
        /// </summary>
        private void DisplayCustomer()
        {
            txtFirstName.Text = cstAddModCust.CustFirstName;
            txtLastName.Text = cstAddModCust.CustLastName;
            txtStreetNum.Text = cstAddModCust.CustStreetNum.ToString();
            txtStreetName.Text = cstAddModCust.CustStreetName;
            txtCity.Text = cstAddModCust.CustCity;
            cboStates.Text = cstAddModCust.CustState;
            txtPhone.Text = cstAddModCust.CustPhone;
            if (cstAddModCust.CustReferredBy == 0)
            {
                txtReferred.Text = null;
            }
            else
            {
                txtReferred.Text = cstAddModCust.CustReferredBy.ToString();
            }
        }

        /// <summary>
        /// This event handler for the accept button validates the
        /// data inputted into the textboxes, sets the properties of
        /// a newly instantiated customer object using the information
        /// in the text boxes, then passes the customer information along
        /// to the CustomerDB class to be written to the database. If an 
        /// existing customer is being modified, the event handler will display
        /// an error message if another user has updated the customer
        /// before the user's changes could be pushed through.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                if (blnAddCustomer)
                {
                    cstAddModCust = new Customer();
                    PutCustomerData(cstAddModCust);
                    try
                    {                
                        cstAddModCust.CustID = CustomerDB.AddCustomer(cstAddModCust);
                        DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
                else
                {
                    Customer cstModdedCust = new Customer();
                    cstModdedCust.CustID = cstAddModCust.CustID;
                    PutCustomerData(cstModdedCust);
                    try
                    {
                        if (!CustomerDB.UpdateCustomer(cstAddModCust, cstModdedCust))
                        {
                            MessageBox.Show("Another user has updated or " +
                                "deleted that customer.", "Database Error");
                            DialogResult = DialogResult.Retry;
                        }
                        else
                        {
                            cstAddModCust = cstModdedCust;
                            DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
            }
        }

        /// <summary>
        /// This method validates the data that has been inputted into
        /// the textboxes according to various validation methods present
        /// in the Validator class.
        /// </summary>
        /// <returns>Boolean true or false depending on whether or not
        /// the textboxes pass the validation requirements.</returns>
        private bool IsValidData()
        {
            return
                Validator.IsPresent(txtFirstName) &&
                Validator.IsCleanString(txtFirstName) &&
                Validator.IsPresent(txtLastName) &&
                Validator.IsCleanString(txtLastName) &&
                Validator.IsPresent(txtStreetNum) &&
                Validator.IsInt32(txtStreetNum) &&
                Validator.IsPresent(txtStreetName) &&
                Validator.IsCleanString(txtStreetName) &&
                Validator.IsPresent(txtCity) &&
                Validator.IsCleanString(txtCity) &&
                Validator.IsPresent(cboStates) &&
                Validator.IsPresent(txtPhone) &&
                Validator.IsPhoneNum(txtPhone) &&
                Validator.OptionalIntCheck(txtReferred);        

        }

        /// <summary>
        /// This method places the customer data present in the
        /// textboxes into a customer object that is passed along to the
        /// method.
        /// </summary>
        /// <param name="Customer"> A customer object that will receive
        /// the property defitions based on the information in the textboxes.</param>
        private void PutCustomerData(Customer Customer)
        {
            Customer.CustFirstName = txtFirstName.Text;
            Customer.CustLastName = txtLastName.Text;
            Customer.CustStreetNum = Convert.ToInt32(txtStreetNum.Text);
            Customer.CustStreetName = txtStreetName.Text;
            Customer.CustCity = txtCity.Text;
            Customer.CustState = cboStates.Text;
            Customer.CustPhone = txtPhone.Text;
            if (txtReferred.Text != "")
            Customer.CustReferredBy = Convert.ToInt32(txtReferred.Text);

        }
    }
}
