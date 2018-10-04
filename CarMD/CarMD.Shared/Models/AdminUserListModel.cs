using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shared.Models
{
    public class AdminUserListModel
    {
        public string AdminUserId { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }        
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "You must provide a phone number")]        
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public string LastLoginDateTimeUTC { get; set; }
        public bool? IsActive { get; set; }
        [Required(ErrorMessage = "You must select application type")]
        public int? ApplicationType { get; set; }
        public string ApplicationName { get; set; }
    }

    public class AdminUserListWhereModel
    {
        public string AdminUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> IsSystemAdministrator { get; set; }
        public string LastLoginDateTimeUTC { get; set; }
        public bool? IsActive { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public int? ApplicationType { get; set; }
        public string ApplicationName { get; set; }
    }

    public class AdminUserListParameter
    {
        public bool IsActive { get; set; }
        public DataSourceRequest Request { get; set; }
    }
    public class AdminUserEditViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? PhoneNumber { get; set; }
        public int? Market { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AreasOfExpertise { get; set; }
        public string EmailAddress { get; set; }
        public List<int> RoleName { get; set; }
        public string RoleNameString { get; set; }
        public List<DropDownViewModel> RoleList { get; set; }
        public string PropertyDefinitionDataSetId { get; set; }
        public int? AdminUserType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public bool IsReceivesPastDueNoFixNotifications { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public bool IsActive { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ApplicationType { get; set; }
    }
    public class AdminUserFilterDeleteViewModel
    {
        public int RoleId { get; set; }
        public int RoleManagerId { get; set; }
    }
    public enum EnumApplicationType
    {
        [Description("Admin App")] AdminApp = 1,
        [Description("Master Tech App")] MasterTechApp = 2,
        
    }

}
