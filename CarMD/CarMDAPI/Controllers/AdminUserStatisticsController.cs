using CarMD.Shared.Models;
using DataAccessLayer.Service;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace CarMDAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminUserStatisticsController : ApiController
    {
        AdminUserStaticsService objAdminUserStaticsService = new AdminUserStaticsService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetUserStatisticsForGrid(StatisticsFilter model)
        {
            var query = objAdminUserStaticsService.GetUserStatistics(model);            
            return Request.CreateResponse(HttpStatusCode.OK, query);

        }

        /// <summary>
        /// Used to get the Grid data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetAllUserStatisticsForGrid(StatisticsFilter model)
        {
            var query = objAdminUserStaticsService.GetAllUserStatistics(model);           
            return Request.CreateResponse(HttpStatusCode.OK, query);

        }

        /// <summary>
        /// Get the user list
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        public HttpResponseMessage GetUserList()
        {
            var query = objAdminUserStaticsService.GetUserList();
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }

        public HttpResponseMessage GetActiveUserList()
        {
            var query = objAdminUserStaticsService.GetActiveUserList();
            return Request.CreateResponse(HttpStatusCode.OK, query);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetAdminUserById(AdminUserModel adminUser)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, objAdminUserStaticsService.GetAdminUserById(adminUser.AdminUserID));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}