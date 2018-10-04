using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarMD.Shared.Models;

namespace DataAccessLayer.Service
{
    public class AuthenticationService
    {
        /// <summary>
        /// Validate the user and return the Role of user
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public UserDetails ValidateUserLogin(UserLoginModel objUser)
        {
            UserDetails responseStr = new UserDetails();
            var unitOfWork = new UnitOfWork<CarMDEntities>();           
            var userRepo = unitOfWork.GetRepository<User>();
            var roleRepo = unitOfWork.GetRepository<Role>();
            var userRoleRepo = unitOfWork.GetRepository<UserRoleManager>();

            var query = (from users in userRepo.DoQuery()
                           join roleMgr in userRoleRepo.DoQuery()
                           on users.UserId equals roleMgr.UserId
                           join roleStr in roleRepo.DoQuery()
                           on roleMgr.RoleId equals roleStr.RoleId
                         where users.EmailAddress == objUser.EmailAddress
                         && users.Password == objUser.Password
                           select new UserDetails
                           {
                               UserId = users.UserId,
                               EmailAddress = users.EmailAddress,
                               Response = "User Exist",
                               FirstName = users.FirstName,
                               LastName = users.LastName

                           }).Distinct().SingleOrDefault();
            if (query != null)
            {
                responseStr = query;
                return responseStr;
            }
            else
            {
                responseStr.Response = "User Dose Not Exist";
                return responseStr;
            }
        }

        /// <summary>
        /// Used to get the list of users and list of pages which allowed to the user role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<AppForm> GetAppFormListByUser(UserDetails model) {
            List<AppForm> list = new List<AppForm>();
            List<AppForm> listTemp = new List<AppForm>();
            var unitOfWork = new UnitOfWork<CarMDEntities>();
            var FormPermRepo = unitOfWork.GetRepository<FormPermissionByRoles>();
            var roleRepo= unitOfWork.GetRepository<Role>();
            var appFormRepo = unitOfWork.GetRepository<AppForm>();
            var userRoleRepo = unitOfWork.GetRepository<UserRoleManager>();
            var whereRoleManager = new Specification<UserRoleManager>(x => x.UserId == model.UserId);
            var roleList = userRoleRepo.DoQuery(whereRoleManager).Select(x=>x.RoleId).ToList();
            var appFormList = appFormRepo.DoQuery().Where(x=>x.IsActive == true).ToList();
            if (roleList.Count>0)
            {
                 list = (from rr in roleRepo.DoQuery()
                             join fp in FormPermRepo.DoQuery()
                             on rr.Id equals fp.RoleId
                             join app in appFormRepo.DoQuery()
                             on fp.FormId equals app.FormId
                             where roleList.Contains(rr.RoleId) && app.IsActive == true
                         select app).ToList();

                foreach (var item in list)
                {
                    var appData = appFormList.FirstOrDefault(x => x.FormId == item.ParentMenuId); 
                    if (appData!=null)
                    {
                        listTemp.Add(appData);
                        appData = appFormList.FirstOrDefault(x => x.FormId == appData.ParentMenuId);
                        if (appData!=null)
                        {
                            listTemp.Add(appData);
                        }

                    }
                }
                list.AddRange(listTemp);
                list= list.GroupBy(x => x.FormId).Select(y => y.FirstOrDefault()).ToList();
            }
            return list;
        }
    }
}
