﻿using System;
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

        public bool blnAddCustomer;
        public Customer cstAddModCust;

        private void frmAddModifyCustomer_Load(object sender, EventArgs e)
        {
            if (blnAddCustomer)
            {
                this.Text = "Add Customer";
                cboStates.SelectedIndex = -1;
            }
            else
            {
                this.Text = "Modify Customer";
                this.DisplayCustomer();
            }
        }

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

        //is there a cleaner way to do this? Becker says no
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

        private void PutCustomerData(Customer Customer)
        {
            //make sure these are clean on the way in
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
