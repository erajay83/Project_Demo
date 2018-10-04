using CarMD.Shared.Models;
using DataAccessLayer.Service;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarMDAPI.Controllers
{
    public class ReportsController : ApiController
    {
        #region Diagnostic Report Trends

        DiagnosticReportTrendsService objDiagnosticReportTrendsService = new DiagnosticReportTrendsService();

        /// <summary>
        /// Used to get the records for bind the Grid of trend
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetDiagnosticReportTrendsGrid(DiagnosticReportTrendsModel model)
        {
            var query = objDiagnosticReportTrendsService.GetDiagnosticReportTrendsGrid(model);
            var data = query.ToDataSourceResult(model.Request);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Used to get data for generate the Excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetExcelJasonApi(DiagnosticReportTrendsModel model)
        {
            var query = objDiagnosticReportTrendsService.GetDiagnosticReportTrendsGrid(model);
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }

        /// <summary>
        /// Used to get the list of system list
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        public HttpResponseMessage GetSystemList()
        {
            var query = objDiagnosticReportTrendsService.GetSystemList();
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }
        #endregion

        #region Diagnostic Report Status
        DiagnosticReportsStatsService objDiagnosticReportsStats = new DiagnosticReportsStatsService();

        /// <summary>
        /// Used to get the grid of recallreportgrid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public HttpResponseMessage GetDiagnosticReportsStatsGrid(DiagnosticReportsStatsModel model)
        {
            var query = objDiagnosticReportsStats.GetDiagnosticReportsStatsGrid(model);
            var data = query.ToDataSourceResult(model.Request);
            return Request.CreateResponse(HttpStatusCode.OK, data);
            //return Request.CreateResponse(HttpStatusCode.OK, query);
        }

        /// <summary>
        /// Used to get the list of system list
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public HttpResponseMessage GetDiagnosticReportsStatsSystemList()
        {
            var query = objDiagnosticReportsStats.GetSystemList();
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }

        #endregion

    }
    
}
