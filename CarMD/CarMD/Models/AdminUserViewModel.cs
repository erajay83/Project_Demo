using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMD.Models
{
    public class AdminUserViewModel
    {
        public string AdminUserID { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string EmailAddress { get; set; }
        public long? PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public int? AdminUserType { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public bool IsActive { get; set; }

    }
}