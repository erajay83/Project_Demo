using CarMD.Shared.Models;
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
using Kendo.Mvc.Extensions;
using CarMD.Auth;
using CarMD.Helpers;

namespace CarMD.Controllers.Reports
{
    public class AdminUserStatisticsController : Controller
    {
        Token objToken = new Token();

        /// <summary>
        /// GET: AdminUserStatistics
        /// </summary>
        [AuthorizeUser("AdminUserStatistics")]        
        public ActionResult Index()
        {            
            return View("~/Views/Reports/AdminUserStatistics.cshtml");
        }
        
        /// <summary>
        /// Used to get the AdminUserStatics
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAdminUserStatics([DataSourceRequest] DataSourceRequest request, StatisticsFilter model)
        {
            try
            {
                var token = objToken.GetAPIToken();
                model.Request = request;
                HttpResponseMessage httpResponse;
                model.UserID = model.UserID == "1" ? null : model.UserID;

                // Call web API method
                httpResponse = API.Post.PostObject("AdminUserStatistics/GetAllUserStatisticsForGrid", model, token);
                string response = httpResponse.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<AdminUserStatisticModel>>(response).ToList();                
                return Json(data.ToDataSourceResult(request));
            }
            catch(Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Used to bind the user drop down
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserDropDown()
        {
            try
            {
                // Call web API method
                var token = objToken.GetAPIToken();
                HttpResponseMessage httpResponse = API.Get.APIRequestAll("AdminUserStatistics/GetUserList", token);
                string response = httpResponse.Content.ReadAsStringAsync().Result;
                List<AdminUserModel> modelGrid = JsonConvert.DeserializeObject<List<AdminUserModel>>(response);
                                
                AdminUserModel model = new AdminUserModel();
                model.FirstName = "No Selection";
                model.AdminUserID = "";
                List<AdminUserModel> list = new List<AdminUserModel>();
                list.Add(model);
                list.AddRange(modelGrid);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}