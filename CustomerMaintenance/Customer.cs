using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerMaintenance
{
    /// <summary>
    /// This class represents a customer object that contains
    /// properties according to the specifications provided
    /// by WildCat Pizza.
    /// </summary>
    public class Customer
    {
        private int intCustID;
        private string strCustLastName;
        private string strCustFirstName;
        private int intCustStreetNum;
        private string strCustStreetName;
        private string strCustCity;
        private string strCustState;
        private string strCustPhone;
        private int intCustReferredBy;

        public Customer() { }

        public int CustID
        {
            get
            {
                return intCustID;
            }
            set
            {
                intCustID = value;
            }
        }

        public string CustLastName
        {
            get
            {
                return strCustLastName;
            }
            set
            {
                strCustLastName = value;
            }
        }

        public string CustFirstName
        {
            get
            {
                return strCustFirstName;
            }
            set
            {
                strCustFirstName = value;
            }
        }

        public int CustStreetNum
        {
            get
            {
                return intCustStreetNum;
            }
            set
            {
                intCustStreetNum = value;
            }
        }

        public string CustStreetName
        {
            get
            {
                return strCustStreetName;
            }
            set
            {
                strCustStreetName = value;
            }
        }

        public string CustCity
        {
            get
            {
                return strCustCity;
            }
            set
            {
                strCustCity = value;
            }
        }

        public string CustState
        {
            get
            {
                return strCustState;
            }
            set
            {
                strCustState = value;
            }
        }

        public string CustPhone
        {
            get
            {
                return strCustPhone;
            }
            set
            {
                strCustPhone = value;
            }
        }

        public int CustReferredBy
        {
            get
            {
                return intCustReferredBy;
            }
            set
            {
                intCustReferredBy = value;
            }
        }
    }
}
