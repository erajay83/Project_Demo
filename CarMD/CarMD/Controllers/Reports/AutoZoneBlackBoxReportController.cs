using CarMD.Auth;
using CarMD.Helpers;
using CarMD.Shared.Models;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CarMD.Controllers.Reports
{
    public class AutoZoneBlackBoxReportController : Controller
    {
        // Global define the variables
        Token objToken = new Token();
        private FileInfo uploadedExcelFile;
        private DataTable excelFileDataTable;
        private StringCollection errors = new StringCollection();

        /// <summary>
        ///  GET: AutoZoneBlackBoxReport
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser("AutoZoneBlackBoxReport")]       
        public ActionResult Index()
        {
            return View("~/Views/Reports/AutoZoneBlackBoxReport.cshtml");
        }

        /// <summary>
        /// Used to bind the Grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAutoZoneBlackBoxGrid([DataSourceRequest] DataSourceRequest request, AutoZoneBlackBoxModel model)
        {
            var token = objToken.GetAPIToken();
            var reportTrendsModel = new AutoZoneBlackBoxModel();
          
            // Set parameters to model
            if (!string.IsNullOrEmpty(Request.Params["StartTime"]))
                reportTrendsModel.StartTime = Convert.ToDateTime(Request.Params["StartTime"]);
            else
                reportTrendsModel.StartTime = null;
            if (!string.IsNullOrEmpty(Request.Params["EndTime"]))
                reportTrendsModel.EndTime = Convert.ToDateTime(Request.Params["EndTime"]);
            else
                reportTrendsModel.EndTime = null;
            reportTrendsModel.Request = request;

            // Call web API method
            HttpResponseMessage httpResponse = API.Post.PostObject("AutoZoneBlackBoxReport/GetAutoZoneBlackBoxGrid", reportTrendsModel, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(response);
            List<AutoZoneBlackBoxGrid> partNameObj = JsonConvert.DeserializeObject<List<AutoZoneBlackBoxGrid>>(json["Data"].ToString());
            int Total = JsonConvert.DeserializeObject<int>(json["Total"].ToString());
            IEnumerable<AggregateResult> AggregateResults = JsonConvert.DeserializeObject<IEnumerable<AggregateResult>>(json["AggregateResults"].ToString());
            var data = new GridViewBindResult();
            data.Data = partNameObj;
            data.AggregateResults = AggregateResults;
            data.Total = Total;
            return Json(data);
        }

        /// <summary>
        /// Used to get data for Excel sheet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExcelJason(AutoZoneBlackBoxModel model)
        {
            var token = objToken.GetAPIToken();
            HttpResponseMessage httpResponse = API.Post.PostObject("AutoZoneBlackBoxReport/GetExcelJasonApi", model, token);
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<AutoZoneBlackBoxGrid>>(response).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used to validate excel file upload
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        bool ValidateExcel(HttpPostedFileBase excelFile)
        {
            if (excelFile.ContentLength > 0)
            {
                string tempPath = Server.MapPath("~/Content/Upload/AutoZonePayloads");
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                uploadedExcelFile = new FileInfo(tempPath + "\\" + new FileInfo(excelFile.FileName).Name);

                // Delete a previous upload if it exists
                if (System.IO.File.Exists(uploadedExcelFile.FullName))
                {
                    System.IO.File.Delete(uploadedExcelFile.FullName);
                }

                // Now save the uploaded file
                excelFile.SaveAs(uploadedExcelFile.FullName);

                if (errors.Count > 0)
                {
                    // Since processing falied delete the uploaded file
                    System.IO.File.Delete(uploadedExcelFile.FullName);
                }

                return true;
            }
            else
            {
                errors.Add("The Excel file uploaded was empty. Please check the file and try again.");
                return false;
            }
        }
    }
}