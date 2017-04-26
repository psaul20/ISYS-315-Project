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


//Becker will be actively trying to break our program
namespace CustomerMaintenance
{
    public partial class frmCustomerMaintenance : Form
    {
        public frmCustomerMaintenance()
        {
            InitializeComponent();
        }

        private Customer cstCustomer;

        private void btnGetCustomer_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtCustomerID) &&
                Validator.IsInt32(txtCustomerID))
            {
                int intCustomerID = Convert.ToInt32(txtCustomerID.Text);
                this.GetCustomer(intCustomerID);
                if (cstCustomer == null)
                {
                    MessageBox.Show("No customer found with this ID. " +
                         "Please try again.", "Customer Not Found");
                    this.ClearControls();
                }
                else
                    this.DisplayCustomer();
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
                this.DisplayCustomer();
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
