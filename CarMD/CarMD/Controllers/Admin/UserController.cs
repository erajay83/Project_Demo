using CarMD.Auth;
using CarMD.Helpers;
using CarMD.Models;
using CarMD.Shared.Models;
using DataAccessLayer.Model;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CarMD.Controllers.Admin
{    
    public class UserController : Controller
    {
        Token objToken = new Token();
        
        #region Admin User List

        /// <summary>
        /// Used to get the Admin User list
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser("AdminUserList")]
        public ActionResult AdminUserList()
        {
            return View("~/Views/Admin/AdminUserList.cshtml");
        }

        /// <summary>
        /// Get admin user list for fill grid with filters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="missingLanguage"></param>
        /// <param name="keyword"></param>
        /// <param name="isActive"></param>
        /// <param name="isInactive"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAdminUserListForGrid([DataSourceRequest] DataSourceRequest request)
        {
            var token = objToken.GetAPIToken();
            var model = new AdminUserListParameter();

            // Set the values into model
            model.IsActive = Convert.ToBoolean(Request.Params["IsActive"]);
            model.Request = request;

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("User/GetAdminUserListForGrid", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(response);
            List<AdminUserListModel> partNameObj = JsonConvert.DeserializeObject<List<AdminUserListModel>>(json["Data"].ToString());

            for (int i = 0; i < partNameObj.Count;i++)
            {
                //partNameObj[i].LastLoginDateTimeUTC = (Convert.ToDateTime(partNameObj[i].LastLoginDateTimeUTC).Date).ToString().Split(' ')[0] + " " + Convert.ToDateTime(partNameObj[i].LastLoginDateTimeUTC).ToString("hh:mm tt");
                partNameObj[i].LastLoginDateTimeUTC = "N/A";
            }

            int Total = JsonConvert.DeserializeObject<int>(json["Total"].ToString());
            IEnumerable<AggregateResult> AggregateResults = JsonConvert.DeserializeObject<IEnumerable<AggregateResult>>(json["AggregateResults"].ToString());            
            var data = new GridViewBindResult();
            data.Data = partNameObj;
            data.AggregateResults = AggregateResults;
            data.Total = Total;
            return Json(data);
        }

        /// <summary>
        /// Insert admin user record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAdminUserListDetail()
        {
            var token = objToken.GetAPIToken();
            AdminUserListWhereModel model = new AdminUserListWhereModel();

            // Bind values into model
            model.FirstName = Request.Params["models[0].FirstName"];
            model.LastName = Request.Params["models[0].LastName"];
            model.EmailAddress = Request.Params["models[0].EmailAddress"];
            model.PhoneNumber = (Request.Params["models[0].PhoneNumber"]);
            model.IsSystemAdministrator = Convert.ToBoolean(Request.Params["models[0].IsSystemAdministrator"]) == true ? 1 : 0;
            model.LastLoginDateTimeUTC = Request.Params["models[0].LastLoginDateTimeUTC"];
            model.IsActive = Convert.ToBoolean(Request.Params["models[0].IsActive"]) == true ? true : false;
            model.AdminUserId = Request.Params["models[0].AdminUserId"];

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("User/AddAdminUserList", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            bool result = JsonConvert.DeserializeObject<bool>(response);
            return Json(result);

        }
        /// <summary>
        /// Update Admin User record 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateAdminUserListDetail()
        {
            var token = objToken.GetAPIToken();
            AdminUserListWhereModel model = new AdminUserListWhereModel();

            // Bind values into model
            model.FirstName = Request.Params["models[0].FirstName"];
            model.LastName = Request.Params["models[0].LastName"];
            model.EmailAddress = Request.Params["models[0].EmailAddress"];
            model.PhoneNumber = (Request.Params["models[0].PhoneNumber"]);
            model.IsSystemAdministrator = Convert.ToBoolean(Request.Params["models[0].IsSystemAdministrator"]) == true? 1: 0;
            model.LastLoginDateTimeUTC = Request.Params["models[0].LastLoginDateTimeUTC"];
            model.IsActive = Convert.ToBoolean(Request.Params["models[0].IsActive"]) == true ? true : false;
            model.AdminUserId = Request.Params["models[0].AdminUserId"];

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("User/UpdateAdminUserList", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            bool result = JsonConvert.DeserializeObject<bool>(response);
            return Json(result);
        }
        /// <summary>
        /// delete part name record by Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAdminUserList()
        {
            var token = objToken.GetAPIToken();
            AdminUserListWhereModel model = new AdminUserListWhereModel();

            // Bind values into model
            model.AdminUserId = Request.Params["models[0].AdminUserId"];

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("User/DeleteAdminUserList", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            bool result = JsonConvert.DeserializeObject<bool>(response);
            return Json(result);

        }
        /// <summary>
        /// Used to add edit the user details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AuthorizeUser("AdminUserAddEdit")]
        public ActionResult AdminUserAddEdit(string Id = "")
        {
            var token = objToken.GetAPIToken();
            AdminUserEditViewModel objUser = new AdminUserEditViewModel();
            objUser.UserId = Id;
            HttpResponseMessage httpResponse = API.Post.PostObject("User/GetAdminUserDetailByUserId", objUser, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            AdminUserEditViewModel model = JsonConvert.DeserializeObject<AdminUserEditViewModel>(response);

            //model = AddListInTechViewModel(model);
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.PageTitle = "Add";
            }
            else
            {
                ViewBag.PageTitle = "Edit";

            }
            ViewBag.UserId = Id;
            return View(model);
        }

        /// <summary>
        /// Used to add edit the user details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminUserAddEdit(string Id, AdminUserEditViewModel model)
        {
            var token = objToken.GetAPIToken();
            HttpResponseMessage httpResponse = API.Post.PostObject("User/AddEditAdminUser", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<bool>(response);
            if (result)
            {
                return RedirectToAction("AdminUserList");
            }
            
            ViewBag.UserId = model.UserId;
            return View(model);
        }
        
        #endregion
    }
}