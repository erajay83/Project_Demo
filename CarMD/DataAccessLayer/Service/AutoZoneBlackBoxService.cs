using CarMD.Shared.Models;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class AutoZoneBlackBoxService
    {
        /// <summary>
        /// Used to get the Data of Grid
        /// </summary>
        /// <param name="model"></param>
        public List<AutoZoneBlackBoxGrid> GetAutoZoneBlackBoxGrid(AutoZoneBlackBoxModel model)
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var diagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();
            var diagnosticReportIdRepo = unitOfWork.GetRepository<DiagnosticReportExternalSystemReportId>();
            var ExternalSystemRepo = unitOfWork.GetRepository<ExternalSystem>();
            var UserRepo = unitOfWork.GetRepository<User>();
            var VehicleRepo = unitOfWork.GetRepository<Vehicle>();
            var FixNameRepo = unitOfWork.GetRepository<FixName>();
            var DiagnosticReportResultFixrepo = unitOfWork.GetRepository<DiagnosticReportResultFix>();

            // Added ExternalSystemId Hardcoded
            // Get the inital list of records
            var diagnosticList = (from dr in diagnosticReportRepo.DoQuery()
                         join drId in diagnosticReportIdRepo.DoQuery()
                         on dr.DiagnosticReportId equals drId.DiagnosticReportId into leftResult
                         from drIdlist in leftResult.DefaultIfEmpty()
                         join user in UserRepo.DoQuery()
                         on dr.UserId equals user.UserId
                         join es in ExternalSystemRepo.DoQuery()
                         on user.UserTypeExternalId equals es.ExternalSystemId                        
                         where (dr.CreatedDateTimeUTC >= model.StartTime || model.StartTime == null)
                         && (dr.CreatedDateTimeUTC <= model.EndTime || model.EndTime == null)
                         orderby dr.CreatedDateTimeUTC
                         select new AutoZoneBlackBoxGrid
                         {
                             DiagnosticReportId = dr.DiagnosticReportId,
                             DiagnosticReportResultId = dr.DiagnosticReportResultId,
                             VehicleId = dr.VehicleId,
                             ExternalSystemReportId = drIdlist.ExternalSystemReportId,
                             ReportDate = dr.CreatedDateTimeUTC,
                             ReportTime = dr.CreatedDateTimeUTC,                           
                             ToolLEDStatus = dr.ToolLEDStatus,
                             
                         }).ToList();

            // Gettting vehical list based on inital list of records
            var vehicleList = (from dlist in diagnosticList
                               join vh in VehicleRepo.DoQuery()
                               on dlist.VehicleId equals vh.VehicleId 
                               select new AutoZoneBlackBoxGrid
                               {
                                   DiagnosticReportId = dlist.DiagnosticReportId,
                                   Year = vh.Year,
                                   Make = vh.Make,
                                   Model = vh.Model,
                                   EngineType = vh.EngineType,
                                   TransmissionControlType = vh.TransmissionControlType
                               }).ToList();

            // Getting fix name description based on inital list of records
            var fixNameList = (from list in diagnosticList
                               join drf in DiagnosticReportResultFixrepo.DoQuery()
                               on list.DiagnosticReportResultId equals drf.DiagnosticReportResultId
                               join fn in FixNameRepo.DoQuery()
                               on drf.FixNameId equals fn.FixNameId                             
                               select new AutoZoneBlackBoxGrid
                               {
                                   DiagnosticReportId = list.DiagnosticReportId,
                                   Description = fn.Description
                               }).ToList();

            // Final binding of list
            var finalList = (from dlist in diagnosticList
                            join vlist in vehicleList
                            on dlist.DiagnosticReportId equals vlist.DiagnosticReportId
                             join flist in fixNameList
                            on dlist.DiagnosticReportId equals flist.DiagnosticReportId
                             select new AutoZoneBlackBoxGrid
                            {
                                 ExternalSystemReportId = dlist.ExternalSystemReportId,
                                 ReportDateStr = dlist.ReportDate.ToShortDateString(),
                                 ReportTimeStr = dlist.ReportTime.ToShortTimeString(),
                                 ToolLEDStatus = dlist.ToolLEDStatus,
                                 Year = vlist.Year,
                                 Make = vlist.Make,
                                 Model = vlist.Model,
                                 EngineType = vlist.EngineType,
                                 TransmissionControlType = vlist.TransmissionControlType,
                                 Description = flist.Description

                             }).ToList();

            return finalList;

        }

    }
}
