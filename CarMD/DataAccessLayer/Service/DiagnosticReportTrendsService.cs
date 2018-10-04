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
    public class DiagnosticReportTrendsService
    {

        /// <summary>
        /// Used to get list of System dropdown
        /// </summary>
        /// <returns></returns>
        public List<TrendDropDowdModel> GetSystemList()
        {
            List<TrendDropDowdModel> objSystemList = new List<TrendDropDowdModel>();
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var userRepo = unitOfWork.GetRepository<User>();

            // Apply limit for get records 500. Remove it once in future.
            var query = (from user in userRepo.DoQuery()
                         select new TrendDropDowdModel
                         {
                             Value = user.FirstName + " " + user.LastName,
                             Id = user.UserTypeExternalId
                         }).Distinct().Take(500).ToList();

            var systemList = query.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            foreach (var rowData in systemList)
            {
                TrendDropDowdModel objRow = new TrendDropDowdModel()
                {
                    Id = rowData.Id,
                    Value = rowData.Value
                };
                objSystemList.Add(objRow);
            }
            return objSystemList;
        }

        /// <summary>
        /// Used to get the Diagnostic Report Trends Grid
        /// </summary>
        /// <param name="model"></param>
        public List<DiagnosticReportTrendsGrid> GetDiagnosticReportTrendsGrid(DiagnosticReportTrendsModel model)
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var diagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();
            var userRepo = unitOfWork.GetRepository<User>();
            List<DiagnosticReportTrendsGrid> objListDiagnosticReportTrendsGrid = new List<DiagnosticReportTrendsGrid>();  
            
            List<int?> userTypeList = new List<int?>();
            userTypeList.Add(0);
            userTypeList.Add(1);
            userTypeList.Add(2);

            // Get total report
            var TotalReport = (from dReport in diagnosticReportRepo.DoQuery()
                               join user in userRepo.DoQuery()
                               on dReport.UserId equals user.UserId
                               where
                               user.IsInternal == false
                               && userTypeList.Contains(user.UserType)
                               && (user.UserTypeExternalId == model.SystemID || model.SystemID == null)
                               && (dReport.CreatedDateTimeUTC >= model.StartDateTime || model.StartDateTime == null)
                               && (dReport.CreatedDateTimeUTC <= model.EndDateTime || model.EndDateTime == null)
                               orderby dReport.CreatedDateTimeUTC.Year descending
                               orderby dReport.CreatedDateTimeUTC.Month descending
                               orderby dReport.CreatedDateTimeUTC.Day descending
                               select new
                               {
                                   DiagnosticReportId = dReport.DiagnosticReportId,
                                   CreatedDateTimeUTC = dReport.CreatedDateTimeUTC

                               }).ToList();

            // Get fix Reports
            var fixReportsList = (from fixR in diagnosticReportRepo.DoQuery()
                                  join user in userRepo.DoQuery()
                                    on fixR.UserId equals user.UserId
                                  where fixR.ToolLEDStatus == 3 && fixR.PwrDiagnosticReportFixStatusWhenCreated == 1
                                  && user.IsInternal == false
                                  && userTypeList.Contains(user.UserType)
                                  && (user.UserTypeExternalId == model.SystemID || model.SystemID == null)
                                  && (fixR.CreatedDateTimeUTC >= model.StartDateTime || model.StartDateTime == null)
                                  && (fixR.CreatedDateTimeUTC <= model.EndDateTime || model.EndDateTime == null)
                                  select new
                                  {
                                      DiagnosticReportId = fixR.DiagnosticReportId,
                                      CreatedDateTimeUTC = fixR.CreatedDateTimeUTC

                                  }).ToList();

            // Get fix Reports
            var fixedLaterReportsList = (from fixR in diagnosticReportRepo.DoQuery()
                                         join user in userRepo.DoQuery()
                                         on fixR.UserId equals user.UserId
                                         where fixR.ToolLEDStatus == 3 && fixR.PwrDiagnosticReportFixStatusWhenCreated == 2
                                         && fixR.PwrDiagnosticReportFixStatus == 1
                                         && user.IsInternal == false
                                        && userTypeList.Contains(user.UserType)
                                        && (user.UserTypeExternalId == model.SystemID || model.SystemID == null)
                                        && (fixR.CreatedDateTimeUTC >= model.StartDateTime || model.StartDateTime == null)
                                        && (fixR.CreatedDateTimeUTC <= model.EndDateTime || model.EndDateTime == null)
                                         select new
                                         {
                                             DiagnosticReportId = fixR.DiagnosticReportId,
                                             CreatedDateTimeUTC = fixR.CreatedDateTimeUTC

                                         }).ToList();
            
            var noFixReportsList = (from fixR in diagnosticReportRepo.DoQuery()
                                    join user in userRepo.DoQuery()
                                         on fixR.UserId equals user.UserId
                                    where fixR.ToolLEDStatus == 3
                                    && fixR.PwrDiagnosticReportFixStatus == 2
                                    && user.IsInternal == false
                                    && userTypeList.Contains(user.UserType)
                                    && (user.UserTypeExternalId == model.SystemID || model.SystemID == null)
                                    && (fixR.CreatedDateTimeUTC >= model.StartDateTime || model.StartDateTime == null)
                                    && (fixR.CreatedDateTimeUTC <= model.EndDateTime || model.EndDateTime == null)
                                    select new
                                    {
                                        DiagnosticReportId = fixR.DiagnosticReportId,
                                        CreatedDateTimeUTC = fixR.CreatedDateTimeUTC

                                    }).ToList();
                       

            if (model.GroupBy == "0")
            {
                var totalReportYearCount = TotalReport.GroupBy(x => x.CreatedDateTimeUTC.Year).Select(y => y).ToList();
                var fixReportsYearCount = fixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Year).Select(y => y).ToList();
                var fixedLaterReportsYearCount = fixedLaterReportsList.GroupBy(x => x.CreatedDateTimeUTC.Year).Select(y => y).ToList();
                var noFixReportsYearCount = noFixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Year).Select(y => y).ToList();

                for (int i = 0; i < totalReportYearCount.Count; i++)
                {
                    DiagnosticReportTrendsGrid objDiagnosticReportTrendsGrid = new DiagnosticReportTrendsGrid();
                    objDiagnosticReportTrendsGrid.ReportDate = totalReportYearCount[i].Select(x => x.CreatedDateTimeUTC.Year +"-"+ x.CreatedDateTimeUTC.Month + "-" + x.CreatedDateTimeUTC.Day).FirstOrDefault();
                    objDiagnosticReportTrendsGrid.TotalReports = totalReportYearCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalFixReports = fixReportsYearCount.Count() == 0 ? 0 : fixReportsYearCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalFixProvidedLaterReports = fixedLaterReportsYearCount.Count() == 0 ? 0 : fixedLaterReportsYearCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalNoFixReports = noFixReportsYearCount.Count() == 0 ? 0 : noFixReportsYearCount[i].Count();

                    objListDiagnosticReportTrendsGrid.Add(objDiagnosticReportTrendsGrid);
                }
            }
            else if (model.GroupBy == "1")
            {
                //totalReportYearCount = TotalReport.GroupBy(x => x.CreatedDateTimeUTC.Year).Select(y => y).ToList();
               var totalReportYearMonthCount = TotalReport.GroupBy(x => x.CreatedDateTimeUTC.Month).Select(y => y).ToList();
                var fixReportsYearMonthCount = fixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Month).Select(y => y).ToList();
                var fixedLaterReportsYearMonthCount = fixedLaterReportsList.GroupBy(x => x.CreatedDateTimeUTC.Month).Select(y => y).ToList();
                var noFixReportsYearMonthCount = noFixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Month).Select(y => y).ToList();

                for (int i = 0; i < totalReportYearMonthCount.Count; i++)
                {
                    DiagnosticReportTrendsGrid objDiagnosticReportTrendsGrid = new DiagnosticReportTrendsGrid();
                    objDiagnosticReportTrendsGrid.ReportDate = totalReportYearMonthCount[i].Select(x => x.CreatedDateTimeUTC.Year + "-" + x.CreatedDateTimeUTC.Month + "-" + x.CreatedDateTimeUTC.Day).FirstOrDefault();
                    objDiagnosticReportTrendsGrid.TotalReports = totalReportYearMonthCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalFixReports = fixReportsYearMonthCount.Count() == 0 ? 0 : fixReportsYearMonthCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalFixProvidedLaterReports = fixedLaterReportsYearMonthCount.Count() == 0 ? 0 : fixedLaterReportsYearMonthCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalNoFixReports = noFixReportsYearMonthCount.Count() == 0 ? 0 : noFixReportsYearMonthCount[i].Count();

                    objListDiagnosticReportTrendsGrid.Add(objDiagnosticReportTrendsGrid);
                }
            }
            else
            {
               var totalReportYearMonthDayCount = TotalReport.GroupBy(x => x.CreatedDateTimeUTC.Date).Select(y => y).ToList();                
                var fixReportsYearMonthDayCount = fixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Date).Select(y => y).ToList();
                var fixedLaterReportsYearMonthDayCount = fixedLaterReportsList.GroupBy(x => x.CreatedDateTimeUTC.Date).Select(y => y).ToList();
                var noFixReportsYearMonthDayCount = noFixReportsList.GroupBy(x => x.CreatedDateTimeUTC.Date).Select(y => y).ToList();
                for (int i = 0; i < totalReportYearMonthDayCount.Count; i++)
                {
                    DiagnosticReportTrendsGrid objDiagnosticReportTrendsGrid = new DiagnosticReportTrendsGrid();
                    objDiagnosticReportTrendsGrid.ReportDate = totalReportYearMonthDayCount[i].Select(x => x.CreatedDateTimeUTC.Year + "-" + x.CreatedDateTimeUTC.Month + "-" + x.CreatedDateTimeUTC.Day).FirstOrDefault();
                    objDiagnosticReportTrendsGrid.TotalReports = totalReportYearMonthDayCount[i].Count();
                    objDiagnosticReportTrendsGrid.TotalFixReports = fixReportsYearMonthDayCount.Count() == 0 ? 0 : (i < fixReportsYearMonthDayCount.Count() ? fixReportsYearMonthDayCount[i].Count() : 0);
                    objDiagnosticReportTrendsGrid.TotalFixProvidedLaterReports = fixedLaterReportsYearMonthDayCount.Count() == 0 ? 0 : (i < fixedLaterReportsYearMonthDayCount.Count() ? fixedLaterReportsYearMonthDayCount[i].Count() : 0);
                    objDiagnosticReportTrendsGrid.TotalNoFixReports = noFixReportsYearMonthDayCount.Count() == 0 ? 0 : (i < noFixReportsYearMonthDayCount.Count() ? noFixReportsYearMonthDayCount[i].Count() : 0);

                    objListDiagnosticReportTrendsGrid.Add(objDiagnosticReportTrendsGrid);
                }
            }

            return objListDiagnosticReportTrendsGrid;
        }
    }
}
