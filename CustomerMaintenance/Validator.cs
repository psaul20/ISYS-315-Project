using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerMaintenance
{
    /// <summary>
    /// Provides static methods for validating data.
    /// </summary>
    public static class Validator
    {
        private static string strTitle = "Entry Error";

        /// <summary>
        /// The title that will appear in dialog boxes.
        /// </summary>
        public static string Title
        {
            get
            {
                return strTitle;
            }
            set
            {
                strTitle = value;
            }
        }

        /// <summary>
        /// Checks whether the user entered data into a text box.
        /// </summary>
        /// <param name="textBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered data.</returns>
        public static bool IsPresent(Control Control)
        {
            if (Control.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                TextBox txtBox = (TextBox)Control;
                if (txtBox.Text == "")
                {
                    MessageBox.Show(txtBox.Tag + " is a required field.", Title);
                    txtBox.Focus();
                    return false;
                }
            }
            else if (Control.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                ComboBox comboBox = (ComboBox)Control;
                if (comboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(comboBox.Tag + " is a required field.", "Entry Error");
                    comboBox.Focus();
                    return false;
                }
            }
            return true;
        }

        public static bool OptionalIntCheck(Control Control)
        {
            if (Control.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                TextBox txtBox = (TextBox)Control;
                if (txtBox.Text == "")
                {
                    return true;
                }
                else
                {                 
                    return IsInt32(txtBox);
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the user entered a decimal value into a text box.
        /// </summary>
        /// <param name="TextBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered a decimal value.</returns>
        public static bool IsDecimal(TextBox TextBox)
        {
            try
            {
                Convert.ToDecimal(TextBox.Text);
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show(TextBox.Tag + " must be a decimal number.", Title);
                TextBox.Focus();
                return false;
            }
        }

        /// <summary>
        /// Checks whether the user entered an int value into a text box.
        /// </summary>
        /// <param name="TextBox">The text box control to be validated.</param>
        /// <returns>True if the user has entered an int value.</returns>
        public static bool IsInt32(TextBox TextBox)
        {
            try
            {
                Convert.ToInt32(TextBox.Text);
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show(TextBox.Tag + " must be an integer.", Title);
                TextBox.Focus();
                return false;
            }
            catch (OverflowException)
            {
                MessageBox.Show(TextBox.Tag + " is outside of acceptable range.", Title);
                TextBox.Focus();
                return false;
            }
        }

        public static bool IsPhoneNum(TextBox TextBox)
        {
            try
            {
                int intPhoneNum = Convert.ToInt32(TextBox.Text);
                if (TextBox.Text.Length == 10 && intPhoneNum > 0)
                    return true;
                else
                {
                    MessageBox.Show(TextBox.Tag + " must be a valid phone number " +
                        "in the following format: 9999999999", Title);
                    TextBox.Focus();
                    return false;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(TextBox.Tag + " must be a valid phone number" +
                    " in the following format: 9999999999", Title);
                TextBox.Focus();
                return false;
            }
        }

        public static bool IsCleanString (TextBox TextBox)
        {
            string strText = TextBox.Text;

            bool blnNotSymbol = true;

            foreach (char c in strText)
            {
                if (c > 64 && c < 91 || c > 96 && c < 123 || c == 39 || c == 32)
                    blnNotSymbol = true;

                else
                {
                    blnNotSymbol = false;
                    break;
                }
            }

            if (blnNotSymbol == false)
            {
                MessageBox.Show(TextBox.Tag + " may not contain numbers or symbols.", Title);
                TextBox.Focus();
                return false;
            }

            //if (strText.Any(char.IsDigit))              
            //{
            //    MessageBox.Show(TextBox.Tag + " may not contain numbers.", Title);
            //    TextBox.Focus();
            //    return false;
            //}
            
            //else if (strText.Length > 50)
            //{
            //    MessageBox.Show(TextBox.Tag + " may not be more than 50 characters in length.", Title);
            //    TextBox.Focus();
            //    return false;
            //}

            else
            return true;

        }

        /// <summary>
        /// Checks whether the user entered a value within a specified range into a text box.
        /// </summary>
        /// <param name="TextBox">The text box control to be validated.</param>
        /// <param name="Min">The minimum value for the range.</param>
        /// <param name="Max">The maximum value for the range.</param>
        /// <returns>True if the user has entered a value within the specified range.</returns>
        public static bool IsWithinRange(TextBox TextBox, decimal Min, decimal Max)
        {
            decimal number = Convert.ToDecimal(TextBox.Text);
            if (number < Min || number > Max)
            {
                MessageBox.Show(TextBox.Tag + " must be between " + Min.ToString()
                    + " and " + Max.ToString() + ".", Title);
                TextBox.Focus();
                return false;
            }
            return true;
        }
    }
}