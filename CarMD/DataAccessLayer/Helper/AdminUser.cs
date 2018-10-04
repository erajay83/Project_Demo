
using Innova;
using Innova.DiagnosticReports;
using Innova.Markets;
using Innova.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Innova.Users
{
    public class AdminUser : InnovaBusinessObjectBase, IUser
    {
        private string firstName = "";
        private string lastName = "";
        private string emailAddress = "";
        private string phoneNumber = "";
        private string password = "";
        private string address1 = "";
        private string address2 = "";
        private string city = "";
        private string state = "";
        private string postalCode = "";
        private string shippingAddress1 = "";
        private string shippingAddress2 = "";
        private string shippingCity = "";
        private string shippingState = "";
        private string shippingPostalCode = "";
        private string areasOfExpertise = "";
        private NullableDecimal validationPayRateDollarsPerHour = NullableDecimal.Null;
        private NullableDateTime lastLoginDateTimeUTC = NullableDateTime.Null;
        private AdminUserType adminUserType;
        private bool isSystemAdministrator;
        private bool isReceivesPastDueNoFixNotifications;
        private Market market;
        private string permissions;
        private bool isActive;
        private bool isDeleted;
        private DateTime updatedDateTimeUTC;
        private DateTime createdDateTimeUTC;
        private PropertyDefinitionDataSet propertyDefinitionDataSet;
        private bool enablePropertyDefinitionEditingOnForms;
        private ValidationTestUserAssignmentCollection validationTestUserAssignments;
        private AdminUserRoleCollection adminUserRoles;
        private int validationTestResultsTotalMinutesToComplete;
        private Decimal validationTestResultsPaymentAmount;
        private bool isObjectDirty;
        private bool isObjectLoaded;
        private bool isObjectCreated;
        private bool adminUserRolesUpdated;
        private List<string> permissionsCollection;

        protected internal AdminUser()
        {
            this.IsObjectCreated = true;
            this.IsObjectLoaded = true;
            this.IsObjectDirty = true;
        }

        protected internal AdminUser(Guid id)
          : base(id)
        {
            this.id = id;
        }

        public new bool IsObjectLoaded
        {
            get
            {
                return this.isObjectLoaded;
            }
            set
            {
                this.isObjectLoaded = value;
            }
        }

        public new bool IsObjectDirty
        {
            get
            {
                return this.isObjectDirty;
            }
            set
            {
                this.isObjectDirty = value;
                if (this.isObjectDirty)
                    return;
                this.isObjectCreated = false;
            }
        }

        public new bool IsObjectCreated
        {
            get
            {
                return this.isObjectCreated;
            }
            set
            {
                this.isObjectCreated = value;
            }
        }

        [PropertyDefinition("First Name", "Admin's first name.")]
        public string FirstName
        {
            get
            {
                this.EnsureLoaded();
                return this.firstName;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.firstName != value))
                    return;
                this.IsObjectDirty = true;
                this.firstName = value;
            }
        }

        [PropertyDefinition("Last Name", "Admin's last name.")]
        public string LastName
        {
            get
            {
                this.EnsureLoaded();
                return this.lastName;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.lastName != value))
                    return;
                this.IsObjectDirty = true;
                this.lastName = value;
            }
        }

        [PropertyDefinition("Name", "Admin's full first and last name.")]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
            set
            {
                string[] strArray = value.Split(' ');
                if (strArray.Length == 0)
                    return;
                this.FirstName = strArray[0];
                if (strArray.Length <= 1)
                    return;
                this.LastName = strArray[1];
            }
        }

        [PropertyDefinition("Email Address", "Admin's contact email address")]
        public string EmailAddress
        {
            get
            {
                this.EnsureLoaded();
                return this.emailAddress;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.emailAddress != value))
                    return;
                this.IsObjectDirty = true;
                this.emailAddress = value;
            }
        }

        [PropertyDefinition("Phone Number", "Admin's contact telephone number")]
        public string PhoneNumber
        {
            get
            {
                this.EnsureLoaded();
                return this.phoneNumber;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.phoneNumber != value))
                    return;
                this.IsObjectDirty = true;
                this.phoneNumber = value;
            }
        }

        [PropertyDefinition("Password", "Admin's password, leave blank when editing to keep password the same.")]
        public string Password
        {
            get
            {
                this.EnsureLoaded();
                return this.password;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.password != value))
                    return;
                this.IsObjectDirty = true;
                this.password = value;
            }
        }

        [PropertyDefinition("Address 1", "First line of admin's address.")]
        public string Address1
        {
            get
            {
                this.EnsureLoaded();
                return this.address1;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.address1 != value))
                    return;
                this.IsObjectDirty = true;
                this.address1 = value;
            }
        }

        [PropertyDefinition("Address 2", "Second line of admin's address")]
        public string Address2
        {
            get
            {
                this.EnsureLoaded();
                return this.address2;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.address2 != value))
                    return;
                this.IsObjectDirty = true;
                this.address2 = value;
            }
        }

        [PropertyDefinition("City", "City where admin is located")]
        public string City
        {
            get
            {
                this.EnsureLoaded();
                return this.city;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.city != value))
                    return;
                this.IsObjectDirty = true;
                this.city = value;
            }
        }

        [PropertyDefinition("State", "State where the admin is located.")]
        public string State
        {
            get
            {
                this.EnsureLoaded();
                return this.state;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.state != value))
                    return;
                this.IsObjectDirty = true;
                this.state = value;
            }
        }

        [PropertyDefinition("Zip/Postal Code", "The Zip code or Postal Code of the admin's location.")]
        public string PostalCode
        {
            get
            {
                this.EnsureLoaded();
                return this.postalCode;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.postalCode != value))
                    return;
                this.IsObjectDirty = true;
                this.postalCode = value;
            }
        }

        [PropertyDefinition("Shipping Address 1", "First line of the admin's shipping address.")]
        public string ShippingAddress1
        {
            get
            {
                this.EnsureLoaded();
                return this.shippingAddress1;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.shippingAddress1 != value))
                    return;
                this.IsObjectDirty = true;
                this.shippingAddress1 = value;
            }
        }

        [PropertyDefinition("Shipping Address 2", "Second line of the admin's shipping address.")]
        public string ShippingAddress2
        {
            get
            {
                this.EnsureLoaded();
                return this.shippingAddress2;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.shippingAddress2 != value))
                    return;
                this.IsObjectDirty = true;
                this.shippingAddress2 = value;
            }
        }

        [PropertyDefinition("City", "City of the admin's shipping address.")]
        public string ShippingCity
        {
            get
            {
                this.EnsureLoaded();
                return this.shippingCity;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.shippingCity != value))
                    return;
                this.IsObjectDirty = true;
                this.shippingCity = value;
            }
        }

        [PropertyDefinition("State", "State for the admin's shipping address.")]
        public string ShippingState
        {
            get
            {
                this.EnsureLoaded();
                return this.shippingState;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.shippingState != value))
                    return;
                this.IsObjectDirty = true;
                this.shippingState = value;
            }
        }

        [PropertyDefinition("Zip/Postal Code", "the Zip code or Postal Code of the admin's Shipping Address.")]
        public string ShippingPostalCode
        {
            get
            {
                this.EnsureLoaded();
                return this.shippingPostalCode;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.shippingPostalCode != value))
                    return;
                this.IsObjectDirty = true;
                this.shippingPostalCode = value;
            }
        }

        [PropertyDefinition("Areas of Expertise", "Admin's background or knowledge base.")]
        public string AreasOfExpertise
        {
            get
            {
                this.EnsureLoaded();
                return this.areasOfExpertise;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.areasOfExpertise != value))
                    return;
                this.IsObjectDirty = true;
                this.areasOfExpertise = value;
            }
        }

        [PropertyDefinition("Account Type", "What type of account should the admin be considered.")]
        public AdminUserType AdminUserType
        {
            get
            {
                this.EnsureLoaded();
                return this.adminUserType;
            }
            set
            {
                this.EnsureLoaded();
                if (this.adminUserType == value)
                    return;
                this.IsObjectDirty = true;
                this.adminUserType = value;
            }
        }

        [PropertyDefinition("System Administrator", "Is this admin a system administrator")]
        public bool IsSystemAdministrator
        {
            get
            {
                this.EnsureLoaded();
                return this.isSystemAdministrator;
            }
            set
            {
                this.EnsureLoaded();
                if (this.isSystemAdministrator == value)
                    return;
                this.IsObjectDirty = true;
                this.isSystemAdministrator = value;
            }
        }

        [PropertyDefinition("Receive Past Due Notifications For No-Fix Reports", "Should the user receive past due notifications for no fix reports")]
        public bool IsReceivesPastDueNoFixNotifications
        {
            get
            {
                this.EnsureLoaded();
                return this.isReceivesPastDueNoFixNotifications;
            }
            set
            {
                this.EnsureLoaded();
                if (this.isReceivesPastDueNoFixNotifications == value)
                    return;
                this.IsObjectDirty = true;
                this.isReceivesPastDueNoFixNotifications = value;
            }
        }

        [PropertyDefinition("Market", "The user's market.")]
        public Market Market
        {
            get
            {
                this.EnsureLoaded();
                return this.market;
            }
            set
            {
                this.EnsureLoaded();
                if (this.market == value)
                    return;
                this.IsObjectDirty = true;
                this.market = value;
            }
        }

        [PropertyDefinition("Permissions", "What permissions does the admin have.")]
        public string Permissions
        {
            get
            {
                this.EnsureLoaded();
                if (string.IsNullOrEmpty(this.permissions))
                    this.UpdateAndSavePermissions();
                return this.permissions;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.permissions != value))
                    return;
                this.permissionsCollection = (List<string>)null;
                this.IsObjectDirty = true;
                this.permissions = value;
            }
        }

        [PropertyDefinition("Validation Hourly Pay Rate", "The hourly rate for completing validation tests.")]
        public NullableDecimal ValidationPayRateDollarsPerHour
        {
            get
            {
                this.EnsureLoaded();
                return this.validationPayRateDollarsPerHour;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.validationPayRateDollarsPerHour != value))
                    return;
                this.IsObjectDirty = true;
                this.validationPayRateDollarsPerHour = value;
            }
        }

        [PropertyDefinition("Active", "Is admin currently active.")]
        public bool IsActive
        {
            get
            {
                this.EnsureLoaded();
                return this.isActive;
            }
            set
            {
                this.EnsureLoaded();
                if (this.isActive == value)
                    return;
                this.IsObjectDirty = true;
                this.isActive = value;
            }
        }

        [PropertyDefinition("Deleted", "Has the admin been removed from the system.")]
        public bool IsDeleted
        {
            get
            {
                this.EnsureLoaded();
                return this.isDeleted;
            }
            set
            {
                this.EnsureLoaded();
                if (this.isDeleted == value)
                    return;
                this.IsObjectDirty = true;
                this.isDeleted = value;
            }
        }

        [PropertyDefinition("Last Updated", "When the admin was last updated.")]
        public DateTime UpdatedDateTimeUTC
        {
            get
            {
                this.EnsureLoaded();
                return this.updatedDateTimeUTC;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.updatedDateTimeUTC != value))
                    return;
                this.IsObjectDirty = true;
                this.updatedDateTimeUTC = value;
            }
        }

        [PropertyDefinition("Created Date", "Date the admin was created.")]
        public DateTime CreatedDateTimeUTC
        {
            get
            {
                this.EnsureLoaded();
                return this.createdDateTimeUTC;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.createdDateTimeUTC != value))
                    return;
                this.IsObjectDirty = true;
                this.createdDateTimeUTC = value;
            }
        }

        [PropertyDefinition("Last Logged In", "Date the admin last logged in.")]
        public NullableDateTime LastLoginDateTimeUTC
        {
            get
            {
                this.EnsureLoaded();
                return this.lastLoginDateTimeUTC;
            }
            set
            {
                this.EnsureLoaded();
                if (!(this.lastLoginDateTimeUTC != value))
                    return;
                this.IsObjectDirty = true;
                this.lastLoginDateTimeUTC = value;
            }
        }

        [PropertyDefinition("Property Definition Data Set", "The property definition data set currently in use for this user.")]
        public PropertyDefinitionDataSet PropertyDefinitionDataSet
        {
            get
            {
                this.EnsureLoaded();
                return this.propertyDefinitionDataSet;
            }
            set
            {
                this.EnsureLoaded();
                if (this.propertyDefinitionDataSet == value)
                    return;
                this.IsObjectDirty = true;
                this.propertyDefinitionDataSet = value;
                this.UpdatedField("PropertyDefinitionDataSetId");
            }
        }

        [PropertyDefinition("Enable Property Definition Editing On Forms", "Indicator that user should be able to edit the property definitions while browsing the site.")]
        public bool EnablePropertyDefinitionEditingOnForms
        {
            get
            {
                this.EnsureLoaded();
                return this.enablePropertyDefinitionEditingOnForms;
            }
            set
            {
                this.EnsureLoaded();
                if (this.enablePropertyDefinitionEditingOnForms == value)
                    return;
                this.IsObjectDirty = true;
                this.enablePropertyDefinitionEditingOnForms = value;
                this.UpdatedField(nameof(EnablePropertyDefinitionEditingOnForms));
            }
        }

        public int ValidationTestResultsTotalMinutesToComplete
        {
            get
            {
                return this.validationTestResultsTotalMinutesToComplete;
            }
            set
            {
                this.validationTestResultsTotalMinutesToComplete = value;
            }
        }

        public Decimal ValidationTestResultsPaymentAmount
        {
            get
            {
                return this.validationTestResultsPaymentAmount;
            }
            set
            {
                this.validationTestResultsPaymentAmount = value;
            }
        }

        public ValidationTestUserAssignmentCollection ValidationTestUserAssignments
        {
            get
            {
                if (this.validationTestUserAssignments == null)
                {
                    this.validationTestUserAssignments = new ValidationTestUserAssignmentCollection(this.Registry);
                    this.validationTestUserAssignments.LoadByAdminUser(this);
                }
                return this.validationTestUserAssignments;
            }
        }

        [PropertyDefinition("Roles", "What roles does the admin have.")]
        public AdminUserRoleCollection AdminUserRoles
        {
            get
            {
                if (this.adminUserRoles == null)
                {
                    this.adminUserRoles = new AdminUserRoleCollection(this.Registry);
                    this.adminUserRoles.LoadByAdminUser(this);
                }
                return this.adminUserRoles;
            }
        }

        public void AddAdminUserRole(AdminUserRole role)
        {
            if (this.AdminUserRoles.FindByProperty("Id", (object)role.Id) != null)
                return;
            this.AdminUserRoles.Add(role);
            this.adminUserRolesUpdated = true;
        }

        public static AdminUserCollection Search(Registry registry, string orderBy, NullableBoolean getDeletedUsers, bool includeActiveUsers, bool includeInactiveUsers, int sortDirection, int currentPage, int pageSize)
        {
            AdminUserCollection adminUserCollection = new AdminUserCollection(registry);
            SqlProcedureCommand call = new SqlProcedureCommand();
            call.ProcedureName = "AdminUser_LoadBySearch";
            call.AddNVarChar("OrderBy", orderBy);
            if (!getDeletedUsers.IsNull)
                call.AddBoolean("IsDeleted", getDeletedUsers.Value);
            call.AddBoolean("IncludeActiveUsers", includeActiveUsers);
            call.AddBoolean("IncludeInactiveUsers", includeInactiveUsers);
            call.AddInt32("SortDirection", sortDirection);
            call.AddInt32("CurrentPage", currentPage);
            call.AddInt32("PageSize", pageSize);
            adminUserCollection.Load(call, "AdminUserId", true, true, true);
            return adminUserCollection;
        }

        public static AdminUserCollection GetUsersActive(Registry registry)
        {
            AdminUserCollection adminUserCollection = new AdminUserCollection(registry);
            adminUserCollection.Load(new SqlProcedureCommand()
            {
                ProcedureName = "AdminUser_LoadByActive"
            }, "AdminUserId", true, true);
            return adminUserCollection;
        }

        public static AdminUserCollection GetUsersActiveAndReceivesPastDueNotifications(Registry registry)
        {
            AdminUserCollection adminUserCollection = new AdminUserCollection(registry);
            adminUserCollection.Load(new SqlProcedureCommand()
            {
                ProcedureName = "AdminUser_LoadByActiveAndReceivesPastDueNotifications"
            }, "AdminUserId", true, true);
            return adminUserCollection;
        }

        public static AdminUserCollection GetUsersByType(Registry registry, AdminUserType adminUserType)
        {
            AdminUserCollection adminUserCollection = new AdminUserCollection(registry);
            SqlProcedureCommand call = new SqlProcedureCommand();
            call.ProcedureName = "AdminUser_LoadByAdminUserType";
            call.AddInt32("AdminUserType", (int)adminUserType);
            adminUserCollection.Load(call, "AdminUserId", true, true, false);
            return adminUserCollection;
        }

        public static AdminUserCollection SearchForTechAdmins(Registry registry, string orderBy, NullableBoolean getDeletedUsers, bool includeActiveUsers, bool includeInactiveUsers, int sortDirection, int currentPage, int pageSize)
        {
            AdminUserCollection adminUserCollection = new AdminUserCollection(registry);
            SqlProcedureCommand call = new SqlProcedureCommand();
            call.ProcedureName = "AdminUser_LoadBySearchAndTechAdminType";
            call.AddNVarChar("OrderBy", orderBy);
            if (!getDeletedUsers.IsNull)
                call.AddBoolean("IsDeleted", getDeletedUsers.Value);
            call.AddBoolean("IncludeActiveUsers", includeActiveUsers);
            call.AddBoolean("IncludeInactiveUsers", includeInactiveUsers);
            call.AddInt32("TechAdminType", 1);
            call.AddInt32("CarMDAndTechAdminType", 2);
            call.AddInt32("SortDirection", sortDirection);
            call.AddInt32("CurrentPage", currentPage);
            call.AddInt32("PageSize", pageSize);
            adminUserCollection.Load(call, "AdminUserId", true, true, true);
            return adminUserCollection;
        }

        public static AdminUser GetUserByEmailAddressAndPassword(Registry registry, string emailAddress, string password)
        {
            AdminUser adminUser = (AdminUser)null;
            using (SqlDataReaderWrapper dr = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
            {
                dr.ProcedureName = "AdminUser_LoadByEmailAddressAndPassword";
                dr.AddNVarChar("EmailAddress", emailAddress);
                dr.AddNVarChar("Password", password);
                dr.Execute();
                if (dr.Read())
                {
                    adminUser = (AdminUser)registry.CreateInstance(typeof(AdminUser), dr.GetGuid("AdminUserId"));
                    adminUser.SetPropertiesFromDataReader(dr);
                }
            }
            return adminUser;
        }

        public static AdminUser GetUserByEmailAddress(Registry registry, string emailAddress)
        {
            AdminUser adminUser = (AdminUser)null;
            using (SqlDataReaderWrapper dr = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
            {
                dr.ProcedureName = "AdminUser_LoadByEmailAddress";
                dr.AddNVarChar("EmailAddress", emailAddress);
                dr.Execute();
                if (dr.Read())
                {
                    adminUser = (AdminUser)registry.CreateInstance(typeof(AdminUser), dr.GetGuid("AdminUserId"));
                    adminUser.SetPropertiesFromDataReader(dr);
                }
            }
            return adminUser;
        }

        public static List<DiagnosticReportOBDFixAdminUserStatistics> GetOBDFixStats(Registry registry, AdminUserCollection users, DateTime? startDateTimeUTC, DateTime? endDateTimeUTC)
        {
            List<DiagnosticReportOBDFixAdminUserStatistics> adminUserStatisticsList = new List<DiagnosticReportOBDFixAdminUserStatistics>();
            foreach (AdminUser user in (CollectionBase)users)
            {
                using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(registry.ConnectionStringDefault))
                {
                    dataReaderWrapper.ProcedureName = "DiagnosticReportFixFeedback_GetAdminUserStats";
                    dataReaderWrapper.AddGuid("AdminUserId", user.Id);
                    if (startDateTimeUTC.HasValue)
                        dataReaderWrapper.AddDateTime("StartDateTimeUTC", startDateTimeUTC.Value);
                    if (endDateTimeUTC.HasValue)
                        dataReaderWrapper.AddDateTime("EndDateTimeUTC", endDateTimeUTC.Value);
                    dataReaderWrapper.Execute();
                    if (dataReaderWrapper.Read())
                    {
                        DiagnosticReportOBDFixAdminUserStatistics adminUserStatistics = new DiagnosticReportOBDFixAdminUserStatistics(user, dataReaderWrapper.GetInt32("ReportsClosedNewFixAdded"), dataReaderWrapper.GetInt32("ReportsClosedExistingFixSelected"), dataReaderWrapper.GetInt32("ReportsClosedRejected"), dataReaderWrapper.GetInt32("NumOfFixesFromFixReport"), dataReaderWrapper.GetInt32("NumOfDirectFixes"));
                        adminUserStatisticsList.Add(adminUserStatistics);
                    }
                }
            }
            return adminUserStatisticsList;
        }

        public bool HasPermission(string nodeKey)
        {
            if (string.IsNullOrEmpty(nodeKey) || string.IsNullOrEmpty(this.Permissions))
                return false;
            if (this.permissionsCollection == null)
            {
                this.permissionsCollection = new List<string>();
                this.permissionsCollection.AddRange((IEnumerable<string>)this.Permissions.Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries));
            }
            string str1 = nodeKey;
            string[] separator = new string[1] { "," };
            int num = 1;
            foreach (string str2 in new List<string>((IEnumerable<string>)str1.Split(separator, (StringSplitOptions)num)))
            {
                if (!this.permissionsCollection.Contains(str2))
                    return false;
            }
            return true;
        }

        public static void SetValidationTestResultPaymentAmounts(AdminUserCollection usersToUpdate, NullableDateTime submittedStartDateUTC, NullableDateTime submittedEndDateUTC)
        {
            using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(usersToUpdate.Registry.ConnectionStringDefault))
            {
                dataReaderWrapper.ProcedureName = "AdminUser_GetValidationTestResultPaymentAmounts";
                dataReaderWrapper.AddNVarChar("AdminUserXmlGuidList", usersToUpdate.ToXmlGuidList());
                dataReaderWrapper.AddDateTime("SubmittedStartDateUTC", submittedStartDateUTC);
                dataReaderWrapper.AddDateTime("SubmittedEndDateUTC", submittedEndDateUTC);
                dataReaderWrapper.Execute();
                while (dataReaderWrapper.Read())
                {
                    AdminUser byProperty = (AdminUser)usersToUpdate.FindByProperty("Id", (object)dataReaderWrapper.GetGuid("AdminUserId"));
                    if (byProperty != null)
                    {
                        byProperty.ValidationTestResultsTotalMinutesToComplete = dataReaderWrapper.GetInt32("ValidationTestResultsTotalMinutesToComplete");
                        byProperty.ValidationTestResultsPaymentAmount = byProperty.ValidationPayRateDollarsPerHour * (NullableDecimal)((Decimal)byProperty.ValidationTestResultsTotalMinutesToComplete / new Decimal(60));
                    }
                }
            }
        }

        public void UpdateAndSavePermissions()
        {
            List<string> stringList = new List<string>();
            foreach (AdminUserRole adminUserRole in (CollectionBase)this.AdminUserRoles)
            {
                foreach (string str in new List<string>((IEnumerable<string>)adminUserRole.MenuPermissions.Split(new char[1]
                {
          ','
                }, StringSplitOptions.RemoveEmptyEntries)))
                {
                    if (!stringList.Contains(str))
                        stringList.Add(str);
                }
            }
            if (stringList.Count <= 0)
                return;
            this.Permissions = string.Join(",", (IEnumerable<string>)stringList);
            this.Save();
        }

        public void UpdateAdminUserRoleAssignments(AdminUserRoleCollection roles)
        {
            using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(this.Registry.ConnectionStringDefault))
            {
                dataReaderWrapper.ProcedureName = "AdminUser_UpdateAdminUserRolesByXmlGuidList";
                dataReaderWrapper.AddGuid("AdminUserId", this.Id);
                dataReaderWrapper.AddNVarChar("AdminUserRoleXmlGuidList", roles.ToXmlGuidList());
                dataReaderWrapper.ExecuteNonQuery();
            }
        }

        public new void Load()
        {
            this.Load((SqlConnection)null, (SqlTransaction)null, false);
        }

        public new SqlConnection Load(SqlConnection connection, bool isLoadBase)
        {
            this.Load(connection, (SqlTransaction)null, isLoadBase);
            return connection;
        }

        public new SqlTransaction Load(SqlConnection connection, SqlTransaction transaction, bool isLoadBase)
        {
            this.EnsureValidId();
            if (isLoadBase)
                transaction = base.Load(connection, transaction, isLoadBase);
            if (!this.IsObjectLoaded)
            {
                SqlDataReaderWrapper dr = connection != null ? new SqlDataReaderWrapper(connection, false) : new SqlDataReaderWrapper(this.ConnectionString);
                using (dr)
                {
                    this.SetLoadProcedureCall(dr);
                    if (transaction == null)
                        dr.Execute();
                    else
                        dr.Execute(transaction);
                    if (!dr.Read())
                        throw new ApplicationException("Load Failed for type " + this.GetType().ToString() + " using Procedure: " + dr.ProcedureCall);
                    this.LoadPropertiesFromDataReader(dr, isLoadBase);
                }
            }
            return transaction;
        }

        public new void LoadPropertiesFromDataReader(SqlDataReaderWrapper dr, bool isSetBase)
        {
            if (isSetBase)
                base.LoadPropertiesFromDataReader(dr, isSetBase);
            if (!this.IsObjectLoaded)
                this.SetPropertiesFromDataReader(dr);
            this.IsObjectLoaded = true;
        }

        protected new void EnsureLoaded()
        {
            if (this.IsObjectLoaded || this.IsObjectCreated)
                return;
            this.Load();
        }

        protected new void SetLoadProcedureCall(SqlDataReaderWrapper dr)
        {
            dr.ProcedureName = "AdminUser_Load";
            dr.AddGuid("AdminUserId", this.Id);
        }

        protected new void SetPropertiesFromDataReader(SqlDataReaderWrapper dr)
        {
            this.firstName = dr.GetString("FirstName");
            this.lastName = dr.GetString("LastName");
            this.emailAddress = dr.GetString("EmailAddress");
            this.phoneNumber = dr.GetString("PhoneNumber");
            this.password = dr.GetString("Password");
            this.address1 = dr.GetString("Address1");
            this.address2 = dr.GetString("Address2");
            this.city = dr.GetString("City");
            this.state = dr.GetString("State");
            this.postalCode = dr.GetString("PostalCode");
            this.shippingAddress1 = dr.GetString("ShippingAddress1");
            this.shippingAddress2 = dr.GetString("ShippingAddress2");
            this.shippingCity = dr.GetString("ShippingCity");
            this.shippingState = dr.GetString("ShippingState");
            this.shippingPostalCode = dr.GetString("ShippingPostalCode");
            this.areasOfExpertise = dr.GetString("AreasOfExpertise");
            this.adminUserType = (AdminUserType)dr.GetInt32("AdminUserType");
            this.isSystemAdministrator = dr.GetBoolean("IsSystemAdministrator");
            this.isReceivesPastDueNoFixNotifications = dr.GetBoolean("IsReceivesPastDueNoFixNotifications");
            this.market = (Market)dr.GetInt32("Market");
            this.permissions = dr.GetString("Permissions");
            this.validationPayRateDollarsPerHour = dr.GetNullableDecimal("ValidationPayRateDollarsPerHour");
            this.isActive = dr.GetBoolean("IsActive");
            this.isDeleted = dr.GetBoolean("IsDeleted");
            this.updatedDateTimeUTC = dr.GetDateTime("UpdatedDateTimeUTC");
            this.createdDateTimeUTC = dr.GetDateTime("CreatedDateTimeUTC");
            this.lastLoginDateTimeUTC = dr.GetNullableDateTime("LastLoginDateTimeUTC");
            this.propertyDefinitionDataSet = (PropertyDefinitionDataSet)dr.GetBusinessObjectBase(this.Registry, typeof(PropertyDefinitionDataSet), "PropertyDefinitionDataSetId");
            this.enablePropertyDefinitionEditingOnForms = dr.GetBoolean("EnablePropertyDefinitionEditingOnForms");
            if (!dr.IsDBNull("ValidationTestResultsTotalMinutesToComplete"))
                this.validationTestResultsTotalMinutesToComplete = dr.GetInt32("ValidationTestResultsTotalMinutesToComplete");
            this.IsObjectLoaded = true;
        }

        public override SqlTransaction Save(SqlConnection connection, SqlTransaction transaction)
        {
            transaction = base.Save(connection, transaction);
            if (this.IsObjectDirty)
            {
                transaction = this.EnsureDatabasePrepared(connection, transaction);
                using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(connection, false))
                {
                    if (this.IsObjectCreated)
                    {
                        dataReaderWrapper.ProcedureName = "AdminUser_Create";
                        this.CreatedDateTimeUTC = DateTime.UtcNow;
                        this.UpdatedDateTimeUTC = this.CreatedDateTimeUTC;
                    }
                    else
                        dataReaderWrapper.ProcedureName = "AdminUser_Save";
                    dataReaderWrapper.AddGuid("AdminUserId", this.Id);
                    dataReaderWrapper.AddNVarChar("FirstName", this.FirstName);
                    dataReaderWrapper.AddNVarChar("LastName", this.LastName);
                    dataReaderWrapper.AddNVarChar("EmailAddress", this.EmailAddress);
                    dataReaderWrapper.AddNVarChar("PhoneNumber", InnovaFormatting.CleanPhoneNumber(this.PhoneNumber));
                    dataReaderWrapper.AddNVarChar("Password", this.Password);
                    dataReaderWrapper.AddNVarChar("Address1", this.Address1);
                    dataReaderWrapper.AddNVarChar("Address2", this.Address2);
                    dataReaderWrapper.AddNVarChar("City", this.City);
                    dataReaderWrapper.AddNVarChar("State", this.State);
                    dataReaderWrapper.AddNVarChar("PostalCode", this.PostalCode);
                    dataReaderWrapper.AddNVarChar("ShippingAddress1", this.ShippingAddress1);
                    dataReaderWrapper.AddNVarChar("ShippingAddress2", this.ShippingAddress2);
                    dataReaderWrapper.AddNVarChar("ShippingCity", this.ShippingCity);
                    dataReaderWrapper.AddNVarChar("ShippingState", this.ShippingState);
                    dataReaderWrapper.AddNVarChar("ShippingPostalCode", this.ShippingPostalCode);
                    dataReaderWrapper.AddNVarChar("AreasOfExpertise", this.AreasOfExpertise);
                    dataReaderWrapper.AddInt32("AdminUserType", (int)this.AdminUserType);
                    dataReaderWrapper.AddBoolean("IsSystemAdministrator", this.IsSystemAdministrator);
                    dataReaderWrapper.AddBoolean("IsReceivesPastDueNoFixNotifications", this.IsReceivesPastDueNoFixNotifications);
                    dataReaderWrapper.AddInt32("Market", (int)this.Market);
                    dataReaderWrapper.AddNVarChar("Permissions", this.Permissions);
                    dataReaderWrapper.AddDecimal("ValidationPayRateDollarsPerHour", this.ValidationPayRateDollarsPerHour);
                    dataReaderWrapper.AddBoolean("IsActive", this.IsActive);
                    dataReaderWrapper.AddBoolean("IsDeleted", this.IsDeleted);
                    dataReaderWrapper.AddDateTime("UpdatedDateTimeUTC", this.UpdatedDateTimeUTC);
                    dataReaderWrapper.AddDateTime("CreatedDateTimeUTC", this.CreatedDateTimeUTC);
                    dataReaderWrapper.AddDateTime("LastLoginDateTimeUTC", this.LastLoginDateTimeUTC);
                    dataReaderWrapper.AddBusinessObject("PropertyDefinitionDataSetId", (BusinessObjectBase)this.PropertyDefinitionDataSet);
                    dataReaderWrapper.AddBoolean("EnablePropertyDefinitionEditingOnForms", this.EnablePropertyDefinitionEditingOnForms);
                    dataReaderWrapper.Execute(transaction);
                }
                this.IsObjectDirty = false;
            }
            transaction = this.SaveCollections(connection, transaction);
            return transaction;
        }

        protected new SqlTransaction SaveCollections(SqlConnection connection, SqlTransaction transaction)
        {
            return transaction;
        }

        public override SqlTransaction Delete(SqlConnection connection, SqlTransaction transaction)
        {
            this.EnsureValidId();
            transaction = this.EnsureDatabasePrepared(connection, transaction);
            using (SqlDataReaderWrapper dataReaderWrapper = new SqlDataReaderWrapper(connection, false))
            {
                dataReaderWrapper.ProcedureName = "AdminUser_Delete";
                dataReaderWrapper.AddGuid("AdminUserId", this.Id);
                dataReaderWrapper.ExecuteNonQuery(transaction);
            }
            transaction = base.Delete(connection, transaction);
            return transaction;
        }

        //[SpecialName]
        //Guid IUser.get_Id()
        //{
        //  return this.Id;
        //}

        //[SpecialName]
        //void IUser.set_Id(Guid value)
        //{
        //  this.Id = value;
        //}
    }
}
