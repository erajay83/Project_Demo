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
    public class AutoZoneBlackBoxReportController : ApiController
    {
        AutoZoneBlackBoxService objAutoZoneBlackBoxService = new AutoZoneBlackBoxService();

        /// <summary>
        /// Used to get the grid of recallreportgrid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public HttpResponseMessage GetAutoZoneBlackBoxGrid(AutoZoneBlackBoxModel model)
        {
            var query = objAutoZoneBlackBoxService.GetAutoZoneBlackBoxGrid(model);
            var data = query.ToDataSourceResult(model.Request);
            return Request.CreateResponse(HttpStatusCode.OK, data);            
        }

        /// <summary>
        /// Used to get the data for generate Excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetExcelJasonApi(AutoZoneBlackBoxModel model)
        {
            var query = objAutoZoneBlackBoxService.GetAutoZoneBlackBoxGrid(model);
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }
    }
}
