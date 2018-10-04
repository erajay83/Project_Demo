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
    public class DiagnosticReportsStatsService
    {
        /// <summary>
        /// Used to get System drop down Data
        /// </summary>
        /// <returns></returns>
        public List<DiagnosticReportsStatsDDModel> GetSystemList()
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var ExternalSystemRepo = unitOfWork.GetRepository<ExternalSystem>();

            var query = (from list in ExternalSystemRepo.DoQuery()
                         where list.IsActive == 1
                         orderby list.Name
                         select new DiagnosticReportsStatsDDModel
                         {
                             Id = list.ExternalSystemId,
                             Value = list.Name
                         }).ToList();
            return query;
        }

        /// <summary>
        /// Used to get the Grid data of Diagnostic Reports Stats
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<DiagnosticReportsStatsCaculatedValues> GetDiagnosticReportsStatsGrid(DiagnosticReportsStatsModel model)
        {
            // Apply limit 100 recrods 
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var DiagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();
            var UserRepo = unitOfWork.GetRepository<User>();
            var ExternalSystemRepo = unitOfWork.GetRepository<ExternalSystem>();
            var VehicleRepo = unitOfWork.GetRepository<Vehicle>();
            var PolkVehicleYMMERepo = unitOfWork.GetRepository<PolkVehicleYMME>();
            var DiagnosticReportResultRepo = unitOfWork.GetRepository<DiagnosticReportResult>();
            var DiagnosticReportResultFixRepo = unitOfWork.GetRepository<DiagnosticReportResultFix>();
                      
            List<string> makesListList = new List<string>();

            if (model.MakesList != null)
            {
                var makeList = model.MakesList.Split(',');

                foreach (var rowData in makeList)
                {
                    makesListList.Add(rowData);
                }
            }
            
            var ReportsTotal = (from dReport in DiagnosticReportRepo.DoQuery()
                                join user in UserRepo.DoQuery()
                                on dReport.UserId equals user.UserId
                                join es in ExternalSystemRepo.DoQuery()
                                on user.UserTypeExternalId equals es.ExternalSystemId
                                join vh in VehicleRepo.DoQuery()
                                on dReport.VehicleId equals vh.VehicleId
                                join pvYMME in PolkVehicleYMMERepo.DoQuery()
                                on vh.PolkVehicleYMMEId equals pvYMME.PolkVehicleYMMEId
                                where ((es.ExternalSystemId == model.ExternalSystemId && es.ExternalSystemId != null) || model.ExternalSystemId == null) &&
                                (makesListList.Contains(pvYMME.Make) || model.MakesList == null) &&
                                (dReport.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                && (dReport.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                select new { DiagnosticReportId = dReport.DiagnosticReportId }).Distinct().ToList();

            //var ReportsBasic = 0;
            var tempReportsBasic = (from DiagnosticReportBasic in DiagnosticReportRepo.DoQuery()
                                    join user in UserRepo.DoQuery()
                                    on DiagnosticReportBasic.UserId equals user.UserId
                                    join es in ExternalSystemRepo.DoQuery()
                                    on user.UserTypeExternalId equals es.ExternalSystemId
                                    where DiagnosticReportBasic.OrderLineItemId == null
                                && (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                && (DiagnosticReportBasic.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                && (DiagnosticReportBasic.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                select new { DiagnosticReportId = DiagnosticReportBasic.DiagnosticReportId }).Take(100).Distinct().ToList();

            var ReportsBasic = (from dBasic in tempReportsBasic
                                   join dReport in ReportsTotal
                                   on dBasic.DiagnosticReportId equals dReport.DiagnosticReportId
                                   select dBasic
                                  ).ToList();

            //var ReportsAdvanced = 0;
            var tempReportsAdvanced = (from DiagnosticReportAdvanced in DiagnosticReportRepo.DoQuery()
                                       join user in UserRepo.DoQuery()
                                        on DiagnosticReportAdvanced.UserId equals user.UserId
                                       join es in ExternalSystemRepo.DoQuery()
                                       on user.UserTypeExternalId equals es.ExternalSystemId
                                       where DiagnosticReportAdvanced.OrderLineItemId != null
                                       && (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                       && (DiagnosticReportAdvanced.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                        && (DiagnosticReportAdvanced.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                       select new { DiagnosticReportId = DiagnosticReportAdvanced.DiagnosticReportId }
                                   ).Take(100).Distinct().ToList();

            var ReportsAdvanced = (from dAdvanced in tempReportsAdvanced
                                   join dReport in ReportsTotal
                                   on dAdvanced.DiagnosticReportId equals dReport.DiagnosticReportId                                    
                                   select dAdvanced
                                  ).ToList();

            //var ReportsBasicCreatedWithFixNeeded = 0;
            var tempReportsBasicCreatedWithFixNeeded = (from DiagnosticReportBasicCreatedWithFixNeeded in DiagnosticReportRepo.DoQuery()
                                                        join user in UserRepo.DoQuery()
                                                        on DiagnosticReportBasicCreatedWithFixNeeded.UserId equals user.UserId
                                                        join es in ExternalSystemRepo.DoQuery()
                                                        on user.UserTypeExternalId equals es.ExternalSystemId
                                                        where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null) &&
                                                        DiagnosticReportBasicCreatedWithFixNeeded.OrderLineItemId == null &&
                                                        (
                                                            DiagnosticReportBasicCreatedWithFixNeeded.PwrDiagnosticReportFixStatusWhenCreated == 2
                                                           ||
                                                           DiagnosticReportBasicCreatedWithFixNeeded.Obd1DiagnosticReportFixStatusWhenCreated == 2
                                                           ||
                                                           DiagnosticReportBasicCreatedWithFixNeeded.AbsDiagnosticReportFixStatusWhenCreated == 2
                                                           ||
                                                           DiagnosticReportBasicCreatedWithFixNeeded.SrsDiagnosticReportFixStatusWhenCreated == 2
                                                        )
                                                        && (DiagnosticReportBasicCreatedWithFixNeeded.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                                        && (DiagnosticReportBasicCreatedWithFixNeeded.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                                        select new { DiagnosticReportId = DiagnosticReportBasicCreatedWithFixNeeded.DiagnosticReportId } 
                                                        ).Take(100).Distinct().ToList();

            var ReportsBasicCreatedWithFixNeeded = (from dReport in ReportsTotal
                                                    join dWithFixNeeded in tempReportsBasicCreatedWithFixNeeded
                                                    on dReport.DiagnosticReportId equals dWithFixNeeded.DiagnosticReportId
                                                    select dWithFixNeeded).ToList();

            //var ReportsAdvancedCreatedWithFixNeeded = 0;
            var tempReportsAdvancedCreatedWithFixNeeded = (from DiagnosticReportAdvancedCreatedWithFixNeeded in DiagnosticReportRepo.DoQuery()
                                                           join user in UserRepo.DoQuery()
                                                            on DiagnosticReportAdvancedCreatedWithFixNeeded.UserId equals user.UserId
                                                           join es in ExternalSystemRepo.DoQuery()
                                                           on user.UserTypeExternalId equals es.ExternalSystemId
                                                           where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null) &&
                                                           DiagnosticReportAdvancedCreatedWithFixNeeded.OrderLineItemId != null &&
                                                       (
                                                           DiagnosticReportAdvancedCreatedWithFixNeeded.PwrDiagnosticReportFixStatusWhenCreated == 2
                                                          ||
                                                          DiagnosticReportAdvancedCreatedWithFixNeeded.Obd1DiagnosticReportFixStatusWhenCreated == 2
                                                          ||
                                                          DiagnosticReportAdvancedCreatedWithFixNeeded.AbsDiagnosticReportFixStatusWhenCreated == 2
                                                          ||
                                                          DiagnosticReportAdvancedCreatedWithFixNeeded.SrsDiagnosticReportFixStatusWhenCreated == 2
                                                       )
                                                       && (DiagnosticReportAdvancedCreatedWithFixNeeded.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                                        && (DiagnosticReportAdvancedCreatedWithFixNeeded.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                                       select new { DiagnosticReportId = DiagnosticReportAdvancedCreatedWithFixNeeded.DiagnosticReportId } 
                                                        ).Take(100).Distinct().ToList();

            var ReportsAdvancedCreatedWithFixNeeded = (from dReport in ReportsTotal
                                                       join dWithFixNeeded in tempReportsAdvancedCreatedWithFixNeeded
                                                       on dReport.DiagnosticReportId equals dWithFixNeeded.DiagnosticReportId
                                                       select dWithFixNeeded).ToList();

            //var ReportsAdvancedCreatedWithFixNeededAndNowFixFound = 0;
            var tempReportsAdvancedCreatedWithFixNeededAndNowFixFound = (from DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound in DiagnosticReportRepo.DoQuery()
                                                                         join user in UserRepo.DoQuery()
                                                                        on DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.UserId equals user.UserId
                                                                         join es in ExternalSystemRepo.DoQuery()
                                                                         on user.UserTypeExternalId equals es.ExternalSystemId
                                                                         where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null) 
                                                                         && DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.OrderLineItemId != null
                                                                         && (
                                                                        (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.PwrDiagnosticReportFixStatusWhenCreated == 2 && DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.PwrDiagnosticReportFixStatus == 1)
                                                                        ||
                                                                        (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.Obd1DiagnosticReportFixStatusWhenCreated == 2 && DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.Obd1DiagnosticReportFixStatus == 1)
                                                                        ||
                                                                        (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.AbsDiagnosticReportFixStatusWhenCreated == 2 && DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.AbsDiagnosticReportFixStatus == 1)
                                                                        ||
                                                                        (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.SrsDiagnosticReportFixStatusWhenCreated == 2 && DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.SrsDiagnosticReportFixStatus == 1)
                                                                     )
                                                                     && (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                                                     && (DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                                                     select new { DiagnosticReportId = DiagnosticReportAdvancedCreatedWithFixNeededAndNowFixFound.DiagnosticReportId } 
                                                                    ).Take(100).Distinct().ToList();

            var ReportsAdvancedCreatedWithFixNeededAndNowFixFound = (from dReport in ReportsTotal
                                                                     join dNowFixFound in tempReportsAdvancedCreatedWithFixNeededAndNowFixFound
                                                                     on dReport.DiagnosticReportId equals dNowFixFound.DiagnosticReportId
                                                                     select dNowFixFound).ToList();

            //var ReportsGreen = 0;
            var tempReportsGreen = (from DiagnosticReportGreen in DiagnosticReportRepo.DoQuery()
                                join user in UserRepo.DoQuery()
                                on DiagnosticReportGreen.UserId equals user.UserId
                                join es in ExternalSystemRepo.DoQuery()
                                on user.UserTypeExternalId equals es.ExternalSystemId
                                where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                && DiagnosticReportGreen.ToolLEDStatus == 1
                                && (DiagnosticReportGreen.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                && (DiagnosticReportGreen.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                select new { DiagnosticReportId = DiagnosticReportGreen.DiagnosticReportId } 
                                 ).Take(100).Distinct().ToList();

            var ReportsGreen = (from dGreen in tempReportsGreen
                                   join dReport in ReportsTotal
                                   on dGreen.DiagnosticReportId equals dReport.DiagnosticReportId
                                   select dGreen
                                 ).ToList();

            //var ReportsYellow = 0;
            var tempReportsYellow = (from DiagnosticReportYellow in DiagnosticReportRepo.DoQuery()
                                 join user in UserRepo.DoQuery()
                                 on DiagnosticReportYellow.UserId equals user.UserId
                                 join es in ExternalSystemRepo.DoQuery()
                                 on user.UserTypeExternalId equals es.ExternalSystemId
                                 where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                 && DiagnosticReportYellow.ToolLEDStatus == 2
                                 && (DiagnosticReportYellow.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                  && (DiagnosticReportYellow.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                 select new { DiagnosticReportId = DiagnosticReportYellow.DiagnosticReportId } 
                                   ).Take(100).Distinct().ToList();

            var ReportsYellow = (from dYellow in tempReportsYellow
                                 join dReport in ReportsTotal
                                   on dYellow.DiagnosticReportId equals dReport.DiagnosticReportId
                                   select dYellow
                                 ).ToList();

            //var ReportsRed = 0;
            var tempReportsRed = (from DiagnosticReportRed in DiagnosticReportRepo.DoQuery()
                                  join user in UserRepo.DoQuery()
                                  on DiagnosticReportRed.UserId equals user.UserId
                                  join es in ExternalSystemRepo.DoQuery()
                                  on user.UserTypeExternalId equals es.ExternalSystemId
                                  where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                  && DiagnosticReportRed.ToolLEDStatus == 3
                                    && (DiagnosticReportRed.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                    && (DiagnosticReportRed.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                select new { DiagnosticReportId = DiagnosticReportRed.DiagnosticReportId } 
                                   ).Take(100).Distinct().ToList();

            var ReportsRed = (from dRed in tempReportsRed
                              join dReport in ReportsTotal
                              on dRed.DiagnosticReportId equals dReport.DiagnosticReportId
                              select dRed
                                  ).ToList();

            //var ReportsError = 0;
            var tempReportsError = (from DiagnosticReportError in DiagnosticReportRepo.DoQuery()
                                    join user in UserRepo.DoQuery()
                                    on DiagnosticReportError.UserId equals user.UserId
                                    join es in ExternalSystemRepo.DoQuery()
                                    on user.UserTypeExternalId equals es.ExternalSystemId
                                    where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                    && DiagnosticReportError.ToolLEDStatus == 4
                                    && (DiagnosticReportError.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                     && (DiagnosticReportError.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                    select new { DiagnosticReportId = DiagnosticReportError.DiagnosticReportId } 
                                  ).Take(100).Distinct().ToList();

            var ReportsError = (from dError in tempReportsError
                                join dReport in ReportsTotal
                                on dError.DiagnosticReportId equals dReport.DiagnosticReportId
                                select dError
                                  ).ToList();

            //var ReportsTechRequested = 0;
            var tempReportsTechRequested = (from DiagnosticReportTechRequested in DiagnosticReportRepo.DoQuery()
                                            join user in UserRepo.DoQuery()
                                            on DiagnosticReportTechRequested.UserId equals user.UserId
                                            join es in ExternalSystemRepo.DoQuery()
                                            on user.UserTypeExternalId equals es.ExternalSystemId
                                            where (es.ExternalSystemId == model.ExternalSystemId || model.ExternalSystemId == null)
                                            && DiagnosticReportTechRequested.RequestedTechnicianContactDateTimeUTC != null
                                            && (DiagnosticReportTechRequested.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                            && (DiagnosticReportTechRequested.CreatedDateTimeUTC <= model.EndDateUTC || model.EndDateUTC == null)
                                        select new { DiagnosticReportId = DiagnosticReportTechRequested.DiagnosticReportId } 
                                  ).Take(100).Distinct().ToList();

            var ReportsTechRequested = (from dTechRequested in tempReportsTechRequested
                                        join dReport in ReportsTotal
                                        on dTechRequested.DiagnosticReportId equals dReport.DiagnosticReportId
                                        select dTechRequested
                                  ).ToList();

            //var ReportsAdvancedWithRepairProcedure = 0;
            var ReportsAdvancedWithRepairProcedure = (from dReport in DiagnosticReportRepo.DoQuery()
                                                      join user in UserRepo.DoQuery()
                                                      on dReport.UserId equals user.UserId
                                                      join es in ExternalSystemRepo.DoQuery()
                                                      on user.UserTypeExternalId equals es.ExternalSystemId
                                                      join vh in VehicleRepo.DoQuery()
                                                      on dReport.VehicleId equals vh.VehicleId
                                                      join pvYMME in PolkVehicleYMMERepo.DoQuery()
                                                      on vh.PolkVehicleYMMEId equals pvYMME.PolkVehicleYMMEId
                                                      join drrr in DiagnosticReportResultRepo.DoQuery()
                                                       on dReport.DiagnosticReportResultId equals drrr.DiagnosticReportResultId
                                                      join drrrf in DiagnosticReportResultFixRepo.DoQuery()
                                                      on drrr.DiagnosticReportResultId equals drrrf.DiagnosticReportResultId

                                                      where
                                                      drrrf.OrderLineItemId != null &&
                                                      dReport.OrderLineItemId != null &&
                                                      es.ExternalSystemId == model.ExternalSystemId
                                                      && (makesListList.Contains(pvYMME.Make) || model.MakesList == null)
                                                      && (dReport.CreatedDateTimeUTC >= model.StartDateUTC || model.StartDateUTC == null)
                                                      && (dReport.CreatedDateTimeUTC <= model.StartDateUTC || model.StartDateUTC == null)
                                                      select new { DiagnosticReportId = dReport.DiagnosticReportId } 
                                ).Distinct().ToList();

            DiagnosticReportsStatsValues objDiagnosticReportsStatsValues = new DiagnosticReportsStatsValues();
            objDiagnosticReportsStatsValues.ReportsBasic = ReportsBasic.Count();
            objDiagnosticReportsStatsValues.ReportsAdvanced = ReportsAdvanced.Count();
            objDiagnosticReportsStatsValues.ReportsBasicCreatedWithFixNeeded = ReportsBasicCreatedWithFixNeeded.Count();
            objDiagnosticReportsStatsValues.ReportsAdvancedCreatedWithFixNeeded = ReportsAdvancedCreatedWithFixNeeded.Count();
            objDiagnosticReportsStatsValues.ReportsAdvancedCreatedWithFixNeededAndNowFixFound = ReportsAdvancedCreatedWithFixNeededAndNowFixFound.Count();
            objDiagnosticReportsStatsValues.ReportsGreen = ReportsGreen.Count();
            objDiagnosticReportsStatsValues.ReportsYellow = ReportsYellow.Count();
            objDiagnosticReportsStatsValues.ReportsRed = ReportsRed.Count();
            objDiagnosticReportsStatsValues.ReportsError = ReportsError.Count();
            objDiagnosticReportsStatsValues.ReportsTechRequested = ReportsTechRequested.Count();
            objDiagnosticReportsStatsValues.ReportsAdvancedWithRepairProcedure = ReportsAdvancedWithRepairProcedure.Count();
            objDiagnosticReportsStatsValues.ReportsTotal = ReportsTotal.Count();

            return BindDiagnosticReportStats(model, objDiagnosticReportsStatsValues);
        }

        private List<DiagnosticReportsStatsCaculatedValues> BindDiagnosticReportStats(DiagnosticReportsStatsModel model, DiagnosticReportsStatsValues stats)
        {
            DiagnosticReportsStatsGridHeader objHeader = new DiagnosticReportsStatsGridHeader();
            List<string> descriptionList = new List<string>();
            List<string> dayList = new List<string>();
            List<string> weekList = new List<string>();
            List<string> monthList = new List<string>();
            List<string> quarterList = new List<string>();
            List<string> yearList = new List<string>();
            List<string> totalList = new List<string>();

            descriptionList.Add("Basic Reports");
            descriptionList.Add("Advanced Reports");
            descriptionList.Add("Advanced Reports w/Repairs");
            descriptionList.Add("Basic Reports Created With No Fix");
            descriptionList.Add("Advanced Reports Created With No Fix");
            descriptionList.Add("Advanced Reports Created With No Fix But Now Have a Fix");
            descriptionList.Add("Green LED Status Reports");
            descriptionList.Add("Yellow LED Status Reports");
            descriptionList.Add("Red LED Status Reports");
            descriptionList.Add("Error LED Status Reports");
            descriptionList.Add("Technician Contact Requested");
            descriptionList.Add("Total");

            string tempStart = model.StartDateUTC.ToString();
            string tempEnd = model.EndDateUTC.ToString();

            DateTime start = tempStart == "" ? DateTime.Today : Convert.ToDateTime(tempStart);
            DateTime end = tempEnd == "" ? DateTime.Today : Convert.ToDateTime(tempEnd);
            end = end.AddSeconds(1);
            TimeSpan ts = end.Subtract(start);

            int days = (int)ts.TotalDays;
            int weeks = (int)(days / 7);
            int months = this.GetMonthsBetweenDates(start, end);
            int quarters = (int)(months / 3);
            int years = (int)(months / 12);

            objHeader.DaysCount = "(" + days.ToString() + " day" + ((days > 1) ? "s" : "") + ")";
            objHeader.WeeksCount = "(" + weeks.ToString() + " week" + ((weeks > 1) ? "s" : "") + ")";
            objHeader.MonthsCount = "(" + months.ToString() + " month" + ((months > 1) ? "s" : "") + ")";
            objHeader.QuartersCount = "(" + quarters.ToString() + " quarter" + ((quarters > 1) ? "s" : "") + ")";
            objHeader.YearsCount = "(" + years.ToString() + " year" + ((years > 1) ? "s" : "") + ")";

            if (days > 0)
            {
                dayList.Add(((int)(stats.ReportsBasic / days)).ToString());
                dayList.Add(((int)(stats.ReportsAdvanced / days)).ToString());
                dayList.Add(((int)(stats.ReportsAdvancedWithRepairProcedure / days)).ToString());
                dayList.Add(((int)(stats.ReportsBasicCreatedWithFixNeeded / days)).ToString());
                dayList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeeded / days)).ToString());
                dayList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound / days)).ToString());
                dayList.Add(((int)(stats.ReportsGreen / days)).ToString());
                dayList.Add(((int)(stats.ReportsYellow / days)).ToString());
                dayList.Add(((int)(stats.ReportsRed / days)).ToString());
                dayList.Add(((int)(stats.ReportsError / days)).ToString());
                dayList.Add(((int)(stats.ReportsTechRequested / days)).ToString());
                dayList.Add(((int)(stats.ReportsTotal / days)).ToString());
            }

            if (weeks > 0)
            {
                weekList.Add(((int)(stats.ReportsBasic / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsAdvanced / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsAdvancedWithRepairProcedure / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsBasicCreatedWithFixNeeded / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeeded / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsGreen / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsYellow / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsRed / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsError / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsTechRequested / weeks)).ToString());
                weekList.Add(((int)(stats.ReportsTotal / weeks)).ToString());

            }

            if (months > 0)
            {
                monthList.Add(((int)(stats.ReportsBasic / months)).ToString());
                monthList.Add(((int)(stats.ReportsAdvanced / months)).ToString());
                monthList.Add(((int)(stats.ReportsAdvancedWithRepairProcedure / months)).ToString());
                monthList.Add(((int)(stats.ReportsBasicCreatedWithFixNeeded / months)).ToString());
                monthList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeeded / months)).ToString());
                monthList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound / months)).ToString());
                monthList.Add(((int)(stats.ReportsGreen / months)).ToString());
                monthList.Add(((int)(stats.ReportsYellow / months)).ToString());
                monthList.Add(((int)(stats.ReportsRed / months)).ToString());
                monthList.Add(((int)(stats.ReportsError / months)).ToString());
                monthList.Add(((int)(stats.ReportsTechRequested / months)).ToString());
                monthList.Add(((int)(stats.ReportsTotal / months)).ToString());
            }

            if (quarters > 0)
            {
                quarterList.Add(((int)(stats.ReportsBasic / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsAdvanced / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsAdvancedWithRepairProcedure / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsBasicCreatedWithFixNeeded / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeeded / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsGreen / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsYellow / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsRed / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsError / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsTechRequested / quarters)).ToString());
                quarterList.Add(((int)(stats.ReportsTotal / quarters)).ToString());

            }

            if (years > 0)
            {
                yearList.Add(((int)(stats.ReportsBasic / years)).ToString());
                yearList.Add(((int)(stats.ReportsAdvanced / years)).ToString());
                yearList.Add(((int)(stats.ReportsAdvancedWithRepairProcedure / years)).ToString());
                yearList.Add(((int)(stats.ReportsBasicCreatedWithFixNeeded / years)).ToString());
                yearList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeeded / years)).ToString());
                yearList.Add(((int)(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound / years)).ToString());
                yearList.Add(((int)(stats.ReportsGreen / years)).ToString());
                yearList.Add(((int)(stats.ReportsYellow / years)).ToString());
                yearList.Add(((int)(stats.ReportsRed / years)).ToString());
                yearList.Add(((int)(stats.ReportsError / years)).ToString());
                yearList.Add(((int)(stats.ReportsTechRequested / years)).ToString());
                yearList.Add(((int)(stats.ReportsTotal / years)).ToString());
            }

            totalList.Add(stats.ReportsBasic.ToString());
            totalList.Add(stats.ReportsAdvanced.ToString());
            totalList.Add(stats.ReportsAdvancedWithRepairProcedure.ToString());
            totalList.Add(stats.ReportsBasicCreatedWithFixNeeded.ToString());
            totalList.Add(stats.ReportsAdvancedCreatedWithFixNeeded.ToString());
            totalList.Add(stats.ReportsAdvancedCreatedWithFixNeededAndNowFixFound.ToString());

            totalList.Add(stats.ReportsGreen.ToString());
            totalList.Add(stats.ReportsYellow.ToString());
            totalList.Add(stats.ReportsRed.ToString());
            totalList.Add(stats.ReportsError.ToString());
            totalList.Add(stats.ReportsTechRequested.ToString());
            totalList.Add(stats.ReportsTotal.ToString());

            List<DiagnosticReportsStatsCaculatedValues> objDiagnosticReportsStatsCaculatedValues = new List<DiagnosticReportsStatsCaculatedValues>();
            for (int i = -1; i < descriptionList.Count(); i++)
            {
                
                if (i == -1)
                {
                    DiagnosticReportsStatsCaculatedValues obj = new DiagnosticReportsStatsCaculatedValues();
                    obj.DescriptionList = "";
                    obj.DaysList = objHeader.DaysCount;
                    obj.WeeklyList = objHeader.WeeksCount;
                    obj.MonthlyList = objHeader.MonthsCount;
                    obj.QuarterlyList = objHeader.QuartersCount;
                    obj.YearlyList = objHeader.YearsCount;
                    obj.TotalList = "";
                    objDiagnosticReportsStatsCaculatedValues.Add(obj);
                }
                else
                {
                    DiagnosticReportsStatsCaculatedValues obj = new DiagnosticReportsStatsCaculatedValues();
                    obj.DescriptionList = descriptionList[i];
                    obj.DaysList = dayList.Count == 0 ? "0" : dayList[i];
                    obj.WeeklyList = weekList.Count == 0 ? "0" : weekList[i];
                    obj.MonthlyList = monthList.Count == 0 ? "0" : monthList[i];
                    obj.QuarterlyList = quarterList.Count == 0 ? "0" : quarterList[i];
                    obj.YearlyList = yearList.Count == 0 ? "0" : yearList[i];
                    obj.TotalList = totalList.Count == 0 ? "0" : totalList[i];

                    objDiagnosticReportsStatsCaculatedValues.Add(obj);
                }
            }
            
            return objDiagnosticReportsStatsCaculatedValues;
        }

        private int GetMonthsBetweenDates(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public IQueryable<ReviewReportsModel> GetReportReviewForGrid(ReviewReportsModel requestModel)
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var vehicleRepo = unitOfWork.GetRepository<Vehicle>();
            var polkVehicleRepo = unitOfWork.GetRepository<PolkVehicleYMME>();
            var diagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();
            var polkVehicle = (from p in polkVehicleRepo.DoQuery() select p);
            var diagnostic = (from d in diagnosticReportRepo.DoQuery() select d);

            //search all perameter into the tables

            if (requestModel.UpdatedFrom != null && requestModel.UpdatedTo != null)
            {
                diagnostic = diagnostic.Where(f => f.UpdatedDateTimeUTC != null && f.UpdatedDateTimeUTC >= requestModel.UpdatedFrom && f.UpdatedDateTimeUTC <= requestModel.UpdatedTo);
            }

            if (!String.IsNullOrEmpty(requestModel.RoxCode))
            {
                diagnostic = diagnostic.Where(f => !string.IsNullOrEmpty(f.RepairOrderNumber) && f.RepairOrderNumber.ToLower().Contains(requestModel.RoxCode.ToLower()));
            }

            if (requestModel.Year != null && requestModel.Year != 0)
            {
                polkVehicle = polkVehicle.Where(f => f.Year == requestModel.Year);
            }

            if (requestModel.Status > -1)
            {
                diagnostic = diagnostic.Where(f => f.PwrDiagnosticReportFixFeedbackStatus == requestModel.Status && f.SrsDiagnosticReportFixFeedbackStatus == requestModel.Status || f.AbsDiagnosticReportFixStatus == requestModel.Status);
            }

            if (!(string.IsNullOrEmpty(requestModel.Make)) && requestModel.Make != null && requestModel.Make != "")
            {
                polkVehicle = polkVehicle.Where(f => f.Make.ToLower() == requestModel.Make.ToLower());
            }

            if (!(string.IsNullOrEmpty(requestModel.VinCode)) && requestModel.VinCode != null && requestModel.VinCode != "")
            {
                diagnostic = diagnostic.Where(f => f.VIN.ToLower().Contains(requestModel.VinCode.ToLower()));
            }

            if (!(string.IsNullOrEmpty(requestModel.Model)) && requestModel.Model != null && requestModel.Model != "")
            {
                polkVehicle = polkVehicle.Where(f => f.Model.ToLower() == requestModel.Model.ToLower());
            }

            //Implement join and get all data

            var data = (from v in vehicleRepo.DoQuery()
                        join p in polkVehicle
                        on v.PolkVehicleYMMEId equals p.PolkVehicleYMMEId
                        join d in diagnostic
                        on v.VehicleId equals d.VehicleId
                        select new ReviewReportsModel
                        {
                            VehicleId = v.VehicleId,
                            PolkVehicleYMMEId = p.PolkVehicleYMMEId,
                            DiagnosticReportId = d.DiagnosticReportId,
                            DataRange = d.CreatedDateTimeUTC,
                            Through = d.CreatedDateTimeUTC,
                            RoxCode = d.RepairOrderNumber,
                            Year = p.Year,
                            Make = p.Make,
                            Model = p.Model,
                            VinCode = d.VIN,
                            ReportDate = d.CreatedDateTimeUTC,
                            Engine = p.EngineType,
                            Transmission = p.Transmission,
                            Mileage = d.VehicleMileage,
                            LicensePlate = v.LicensePlateNumber,
                            Score = v.EngineVINCode
                        }).Distinct();
            return data;
        }
        public bool DeleteReportList(string VehicleId)
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            try
            {
                var vehicleRepo = unitOfWork.GetRepository<Vehicle>();
                var polkVehicleRepo = unitOfWork.GetRepository<PolkVehicleYMME>();
                var diagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();

                // get all Vehicle records by Id
                var where = new Specification<Vehicle>(f => f.VehicleId == VehicleId);
                var vehicle = vehicleRepo.DoQuery(where).FirstOrDefault();

                if (vehicle != null)
                {
                    //  get all PolkVehicleYMME records by Id
                    var polkmap = new Specification<PolkVehicleYMME>(f => f.PolkVehicleYMMEId == vehicle.PolkVehicleYMMEId);
                    var polkMapList = polkVehicleRepo.DoQuery(polkmap);

                    if (polkMapList.Any())
                    {
                        foreach (var polkMap in polkMapList.ToList())
                        {
                            polkVehicleRepo.Delete(polkMap);
                            unitOfWork.Save();
                        }
                    }

                    // get all DiagnosticReportRepo records by Id
                    var diagnosticReportFixFeedback = new Specification<DiagnosticReport>(f => f.VehicleId == vehicle.VehicleId);
                    var diagnosticReport = diagnosticReportRepo.DoQuery(diagnosticReportFixFeedback);

                    if (diagnosticReport.Any())
                    {
                        foreach (var diagnostic in diagnosticReport.ToList())
                        {
                            diagnosticReportRepo.Delete(diagnostic);
                            unitOfWork.Save();
                        }
                    }

                    vehicleRepo.Delete(vehicle);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}

