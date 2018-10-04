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
    public class AdminUserStaticsService
    {
        /// <summary>
        /// Used to get the dropdown of user
        /// </summary>
        /// <returns></returns>
        public List<AdminUserModel> GetUserList()
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var fixAdminUser = unitOfWork.GetRepository<AdminUser>();
            var diagnosticReportFixRepo = unitOfWork.GetRepository<DiagnosticReportFixFeedback>();

            // Get Userlist
            var query_User = (from d in diagnosticReportFixRepo.DoQuery()
                              join u in fixAdminUser.DoQuery()
                              on d.AdminUserId_FeedbackReviewedBy equals u.AdminUserId
                              select new AdminUserModel
                              {
                                  AdminUserID = d.AdminUserId_FeedbackReviewedBy,
                                  FirstName = u.FirstName +" " + u.LastName                                  
                              }).Distinct().ToList();
                       
            return query_User;
        }

        /// <summary>
        /// Used to get the ALL users of statistics
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<AdminUserStatisticModel> GetAllUserStatistics(StatisticsFilter model)
        {
            List<AdminUserStatisticModel> objListAdminUserStatisticModel = new List<AdminUserStatisticModel>();
            var unitOfWork = new UnitOfWork<CarMDEntities>();         
            var fixAdminUser = unitOfWork.GetRepository<AdminUser>();
            var diagnosticReportFixRepo = unitOfWork.GetRepository<DiagnosticReportFixFeedback>();
            
            if (model.UserID != null)
            {
                StatisticsFilter objStatisticsFilter = new StatisticsFilter()
                {
                    UserID = model.UserID,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                objListAdminUserStatisticModel.Add(GetUserStatistics(objStatisticsFilter));
            }
            else
            {

                // Get Userlist
                var query_User = (from d in diagnosticReportFixRepo.DoQuery()
                              join u in fixAdminUser.DoQuery()
                              on d.AdminUserId_FeedbackReviewedBy equals u.AdminUserId
                              select d).ToList();

                var userDistinct = query_User.GroupBy(x => x.AdminUserId_FeedbackReviewedBy).Select(y => y.FirstOrDefault());

                foreach (var rowData in userDistinct)
                {
                    StatisticsFilter objStatisticsFilter = new StatisticsFilter()
                    {
                        UserID = rowData.AdminUserId_FeedbackReviewedBy,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate
                    };
                    objListAdminUserStatisticModel.Add(GetUserStatistics(objStatisticsFilter));
                }
            }
            return objListAdminUserStatisticModel;
        }

        /// <summary>
        /// Used to get the single admin user statistics
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AdminUserStatisticModel GetUserStatistics(StatisticsFilter model)
        {
            AdminUserStatisticModel objAdminUserStatisticModel = new AdminUserStatisticModel();

            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var diagnosticReportFixRepo = unitOfWork.GetRepository<DiagnosticReportFixFeedback>();
            var diagnosticReportRepo = unitOfWork.GetRepository<DiagnosticReport>();
            var fixRepo = unitOfWork.GetRepository<Fix>();
            var fixAdminUser = unitOfWork.GetRepository<AdminUser>();

            List<int?> codeSystemList = new List<int?>();

            codeSystemList.Add(0);
            codeSystemList.Add(1);
            codeSystemList.Add(2);
            codeSystemList.Add(3);
            
            //User
            var query_User = (from u in fixAdminUser.DoQuery()
                              join d in diagnosticReportFixRepo.DoQuery()
                              on u.AdminUserId equals d.AdminUserId_FeedbackReviewedBy
                              where d.AdminUserId_FeedbackReviewedBy == model.UserID
                              select u
                              ).FirstOrDefault();

            //Reports Closed(New Fix Added)
            var query_ReportsClosedNewFixAdded = (from d in diagnosticReportFixRepo.DoQuery()
                                                  where d.DiagnosticReportFixFeedbackStatus == 3
                                                  && codeSystemList.Contains(d.DiagnosticReportErrorCodeSystemType)
                                                  && (model.StartDate == null || d.FeedbackReviewedDateTimeUTC >= model.StartDate)
                                                  && (model.EndDate == null || d.FeedbackReviewedDateTimeUTC <= model.EndDate)
                                                  && d.AdminUserId_FeedbackReviewedBy == model.UserID
                                                  select d.DiagnosticReportId).Count();

            //Reports Closed(Existing Fix Selected)
            var query_ReportsClosedExistingFixSelected = (from d in diagnosticReportFixRepo.DoQuery()
                                                          where (d.DiagnosticReportFixFeedbackStatus == 4
                                                          && codeSystemList.Contains(d.DiagnosticReportErrorCodeSystemType))
                                                          && (model.StartDate == null || d.FeedbackReviewedDateTimeUTC >= model.StartDate)
                                                          && (model.EndDate == null || d.FeedbackReviewedDateTimeUTC <= model.EndDate)
                                                          && d.AdminUserId_FeedbackReviewedBy == model.UserID
                                                          select d.DiagnosticReportId).Count();

            // Reports Closed(Rejected)
            var query_ReportsClosedRejected = (from d in diagnosticReportFixRepo.DoQuery()
                                               where (d.DiagnosticReportFixFeedbackStatus == 9
                                               && codeSystemList.Contains(d.DiagnosticReportErrorCodeSystemType))
                                               && (model.StartDate == null || d.FeedbackReviewedDateTimeUTC >= model.StartDate)
                                               && (model.EndDate == null || d.FeedbackReviewedDateTimeUTC <= model.EndDate)
                                               && d.AdminUserId_FeedbackReviewedBy == model.UserID
                                               select d.DiagnosticReportId).Count();

            List<int?> FixFeedbackList = new List<int?>();

            FixFeedbackList.Add(3);
            FixFeedbackList.Add(4);

            // # of Fixes from Fix Report
            var query_NumOfFixesFromFixReport = (from d in diagnosticReportRepo.DoQuery()
                                                 join dfix in diagnosticReportFixRepo.DoQuery()
                                                 on d.DiagnosticReportId equals dfix.DiagnosticReportId
                                                 where
                                                 (d.CreatedDateTimeUTC >= model.StartDate || model.StartDate == null)
                                                  &&
                                                 (d.CreatedDateTimeUTC <= model.EndDate || model.EndDate == null)
                                                 &&
                                                 (
                                                    (
                                                       d.AdminUserId_PwrWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_AbsWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_SrsWorkingOnFix == model.UserID
                                                    )
                                                    &&
                                                    (
                                                        d.PwrDiagnosticReportFixStatusWhenCreated == 2 // There was no fix available when report was created
                                                        &&
                                                        dfix.DiagnosticReportErrorCodeSystemType == 0
                                                        &&
                                                        FixFeedbackList.Contains(dfix.DiagnosticReportFixFeedbackStatus) //The new fix provided was approved or an existing fix was selected.
                                                    )
                                                 )
                                                 ||
                                                 (
                                                    (
                                                       d.AdminUserId_PwrWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_AbsWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_SrsWorkingOnFix == model.UserID
                                                    )
                                                    &&
                                                    (
                                                        d.Obd1DiagnosticReportFixStatusWhenCreated == 2 // There was no fix available when report was created
                                                        &&
                                                        dfix.DiagnosticReportErrorCodeSystemType == 1
                                                        &&
                                                        FixFeedbackList.Contains(dfix.DiagnosticReportFixFeedbackStatus)//The new fix provided was approved or an existing fix was selected.
                                                    )
                                                 )
                                                 ||
                                                 (
                                                    (
                                                       d.AdminUserId_PwrWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_AbsWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_SrsWorkingOnFix == model.UserID
                                                    )
                                                    &&
                                                    (
                                                        d.AbsDiagnosticReportFixStatusWhenCreated == 2 // There was no fix available when report was created
                                                        &&
                                                        dfix.DiagnosticReportErrorCodeSystemType == 2
                                                        &&
                                                        FixFeedbackList.Contains(dfix.DiagnosticReportFixFeedbackStatus) //The new fix provided was approved or an existing fix was selected.
                                                    )
                                                 )
                                                 ||
                                                 (
                                                    (
                                                       d.AdminUserId_PwrWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_AbsWorkingOnFix == model.UserID
                                                       ||
                                                       d.AdminUserId_SrsWorkingOnFix == model.UserID
                                                    )
                                                    &&
                                                    (
                                                        d.AbsDiagnosticReportFixStatusWhenCreated == 2 // There was no fix available when report was created
                                                        &&
                                                        dfix.DiagnosticReportErrorCodeSystemType == 3
                                                        &&
                                                        FixFeedbackList.Contains(dfix.DiagnosticReportFixFeedbackStatus) //The new fix provided was approved or an existing fix was selected.
                                                    )
                                                 )
                                                 select d.DiagnosticReportId).Count();


            //# of Direct Fixes
            var query_NumOfDirectFixes = (from f in fixRepo.DoQuery()
                                          where f.CreatedByAdminUserId == model.UserID
                                          && (model.StartDate == null || f.CreatedDateTimeUTC >= model.StartDate)
                                          && (model.EndDate == null || f.CreatedDateTimeUTC >= model.EndDate)
                                          select f.FixId
                                          ).Count();

            // Total Reports
            var NumOfTotalReport = query_ReportsClosedNewFixAdded + query_ReportsClosedExistingFixSelected + query_ReportsClosedRejected;

            if (NumOfTotalReport == 0)
                NumOfTotalReport = query_NumOfFixesFromFixReport;

            if (query_User == null)
                objAdminUserStatisticModel.User = "";
            else
                objAdminUserStatisticModel.User = query_User.FirstName + " " + query_User.LastName;

            objAdminUserStatisticModel.ReportsClosedNewFixAdded = query_ReportsClosedNewFixAdded;
            objAdminUserStatisticModel.ReportsClosedExistingFixSelected = query_ReportsClosedExistingFixSelected;
            objAdminUserStatisticModel.ReportsClosedRejected = query_ReportsClosedRejected;
            objAdminUserStatisticModel.NumOfFixesFromFixReport = query_NumOfFixesFromFixReport;
            objAdminUserStatisticModel.NumOfDirectFixes = query_NumOfDirectFixes;
            objAdminUserStatisticModel.NumOfTotalReport = NumOfTotalReport;

            return objAdminUserStatisticModel;
        }

        public List<AdminUserModel> GetActiveUserList()
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var fixAdminUser = unitOfWork.GetRepository<AdminUser>();

            // Get Userlist
            var query_User = (from u in fixAdminUser.DoQuery()
                              where u.IsActive == 1 && u.IsDeleted == 0
                              select new AdminUserModel
                              {
                                  AdminUserID = u.AdminUserId,
                                  FirstName = u.FirstName + " " + u.LastName
                              }).Distinct().ToList();

            return query_User;
        }

        public AdminUserModel GetAdminUserById(string adminUserId)
        {
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var fixAdminUser = unitOfWork.GetRepository<AdminUser>();

            // Get UserById
            var query_User = (from u in fixAdminUser.DoQuery()
                              where  u.AdminUserId == adminUserId && u.IsActive == 1 && u.IsDeleted == 0
                              select new AdminUserModel
                              {
                                  AdminUserID = u.AdminUserId,
                                  FirstName = u.FirstName,
                                  Lastname = u.LastName,
                                  EmailAddress = u.EmailAddress,
                                  PhoneNumber = u.PhoneNumber,
                                  Address1 = u.Address1,
                                  Address2 = u.Address2,
                                  City = u.City,
                                  State = u.State,
                                  PostalCode = u.PostalCode,
                                  ShippingAddress1 = u.ShippingAddress1,
                                  ShippingAddress2 = u.ShippingAddress2, 
                                  ShippingCity = u.ShippingCity,
                                  ShippingState = u.ShippingState,
                                  ShippingPostalCode = u.ShippingPostalCode,
                                  AdminUserType = u.AdminUserType == null ? null : u.AdminUserType,
                                  IsSystemAdministrator = u.IsSystemAdministrator == 0 ? false : true,
                                  IsActive = u.IsActive == 0 ? false : true,
                              }).Distinct().FirstOrDefault();

            return query_User;
        }
    }
}
