using CarMD.Auth;
using CarMD.Helpers;
using CarMD.Models;
using CarMD.Shared.Models;
using CarMD.ViewModel;
using Kendo.Mvc.Extensions;
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

namespace CarMD.Controllers.Reports
{
    public class ReportsController : Controller
    {
        Token objToken = new Token();

        #region Diagnostic Report Status

        /// <summary>
        /// GET: Diagnostic Report Stats Reports
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser("GetDiagnosticReportStatus")]        
        public ActionResult GetDiagnosticReportStatus()
        {
            return View("~/Views/Reports/DiagnosticReportStats.cshtml");
        }

        /// <summary>
        /// Used to bind the reports of stats into Grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDiagnosticReportsStatsGrid([DataSourceRequest] DataSourceRequest request, DiagnosticReportsStatsModel model)
        {
            var token = objToken.GetAPIToken();
            var reportsStatsModel = new DiagnosticReportsStatsModel();

            // Set the values into model
            reportsStatsModel.Request = request;
            reportsStatsModel.ExternalSystemId = (Request.Params["ExternalSystemId"] == "1" || Request.Params["ExternalSystemId"] == "") ? null : Request.Params["ExternalSystemId"];
            reportsStatsModel.MakesList = (Request.Params["MakesList"] == "1" || Request.Params["MakesList"] == "") ? null : Request.Params["MakesList"];

            if (!string.IsNullOrEmpty(Request.Params["StartDateUTC"]))
                reportsStatsModel.StartDateUTC = Convert.ToDateTime(Request.Params["StartDateUTC"]);
            else
                reportsStatsModel.StartDateUTC = null;
            if (!string.IsNullOrEmpty(Request.Params["EndDateUTC"]))
                reportsStatsModel.EndDateUTC = Convert.ToDateTime(Request.Params["EndDateUTC"]);
            else
                reportsStatsModel.EndDateUTC = null;

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("Reports/GetDiagnosticReportsStatsGrid", reportsStatsModel, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(response);
            List<DiagnosticReportsStatsCaculatedValues> partNameObj = JsonConvert.DeserializeObject<List<DiagnosticReportsStatsCaculatedValues>>(json["Data"].ToString());

            int Total = JsonConvert.DeserializeObject<int>(json["Total"].ToString());
            IEnumerable<AggregateResult> AggregateResults = JsonConvert.DeserializeObject<IEnumerable<AggregateResult>>(json["AggregateResults"].ToString());
            var data = new GridViewBindResult();
            data.Data = partNameObj;
            data.AggregateResults = AggregateResults;
            data.Total = Total;
            return Json(data);
        }

        /// <summary>
        /// Used to bind the dropdown of System
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDiagnosticReportsStatsSystemList()
        {
            try
            {
                // Get token value and call web API method
                var token = objToken.GetAPIToken();
                HttpResponseMessage httpResponse = API.Get.APIRequestAll("Reports/GetDiagnosticReportsStatsSystemList", token);
                string response = httpResponse.Content.ReadAsStringAsync().Result;
                List<DiagnosticReportsStatsDDModel> system = JsonConvert.DeserializeObject<List<DiagnosticReportsStatsDDModel>>(response);
                DiagnosticReportsStatsDDModel model = new DiagnosticReportsStatsDDModel();
                model.Value = "No Selection";
                model.Id = "";
                List<DiagnosticReportsStatsDDModel> list = new List<DiagnosticReportsStatsDDModel>();
                list.Add(model);
                list.AddRange(system);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Used to get the list of Make dropdown
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMakeList()
        {
            try
            {
                // Get token value and call web API method
                var token = objToken.GetAPIToken();
                HttpResponseMessage httpResponse = API.Get.APIRequestAll("VehicleDTCCode/GetMakeList", token);
                string response = httpResponse.Content.ReadAsStringAsync().Result;
                List<DropDownViewModelString> makers = JsonConvert.DeserializeObject<List<DropDownViewModelString>>(response);
                DropDownViewModelString model = new DropDownViewModelString();
                model.Value = "No Selection";
                model.Id = "0";
                List<DropDownViewModelString> list = new List<DropDownViewModelString>();
                list.Add(model);
                list.AddRange(makers);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Diagnostic Report Trends

        /// <summary>
        /// GET: Diagnostic Report Trends Reports
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser("GetDiagnosticReportTrends")]
        public ActionResult GetDiagnosticReportTrends()
        {
            return View("~/Views/Reports/DiagnosticReportTrends.cshtml");
        }

        /// <summary>
        /// Used to bind the grid of trend report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDiagnosticReportTrendsGrid([DataSourceRequest] DataSourceRequest request, DiagnosticReportTrendsModel model)
        {
            var token = objToken.GetAPIToken();
            var reportTrendsModel = new DiagnosticReportTrendsModel();
            reportTrendsModel.Request = request;
            reportTrendsModel.SystemID = (Request.Params["SystemID"] == "1" || Request.Params["SystemID"] == "") ? null : Request.Params["SystemID"];
            reportTrendsModel.GroupBy = Request.Params["GroupBy"];

            if (!string.IsNullOrEmpty(Request.Params["StartDateTime"]))
                reportTrendsModel.StartDateTime = Convert.ToDateTime(Request.Params["StartDateTime"]);
            else
                reportTrendsModel.StartDateTime = null;
            if (!string.IsNullOrEmpty(Request.Params["EndDateTime"]))
                reportTrendsModel.EndDateTime = Convert.ToDateTime(Request.Params["EndDateTime"]);
            else
                reportTrendsModel.EndDateTime = null;

            HttpResponseMessage httpResponse = API.Post.PostObject("Reports/GetDiagnosticReportTrendsGrid", reportTrendsModel, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(response);
            List<DiagnosticReportTrendsGrid> partNameObj = JsonConvert.DeserializeObject<List<DiagnosticReportTrendsGrid>>(json["Data"].ToString());
            int Total = JsonConvert.DeserializeObject<int>(json["Total"].ToString());
            IEnumerable<AggregateResult> AggregateResults = JsonConvert.DeserializeObject<IEnumerable<AggregateResult>>(json["AggregateResults"].ToString());
            var data = new GridViewBindResult();
            data.Data = partNameObj;
            data.AggregateResults = AggregateResults;
            data.Total = Total;
            return Json(data);
        }

        /// <summary>
        /// Used to bind the system dropdown
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSystemList()
        {
            try
            {
                var token = objToken.GetAPIToken();
                HttpResponseMessage httpResponse = API.Get.APIRequestAll("Reports/GetSystemList", token);
                string response = httpResponse.Content.ReadAsStringAsync().Result;
                List<TrendDropDowdModel> system = JsonConvert.DeserializeObject<List<TrendDropDowdModel>>(response);
                TrendDropDowdModel model = new TrendDropDowdModel();
                model.Value = "No Selection";
                model.Id = "";
                List<TrendDropDowdModel> list = new List<TrendDropDowdModel>();
                list.Add(model);
                list.AddRange(system);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Used to get the record for generate the Excel for download
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExcelJason(DiagnosticReportTrendsModel model)
        {
            var token = objToken.GetAPIToken();
            HttpResponseMessage httpResponse = API.Post.PostObject("Reports/GetExcelJasonApi", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<DiagnosticReportTrendsGrid>>(response).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}