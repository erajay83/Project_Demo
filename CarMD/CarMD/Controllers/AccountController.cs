using CarMD.Auth;
using CarMD.Helpers;
using CarMD.Shared.Models;
using DataAccessLayer.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace CarMD.Controllers
{
    public class AccountController : Controller
    {
        [AuthorizeUser]
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            

            return View();
        }

        /// <summary>
        /// Used to process login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                //string emailAddress = "drake4bama08@yahoo.com";
                //string password = "Jeeo";

                // Call web API method for login process
                HttpResponseMessage httpResponse = API.Post.PostObject("Account/Login", model, "");

                string response = httpResponse.Content.ReadAsStringAsync().Result;
                UserDetails objUserDetails = JsonConvert.DeserializeObject<UserDetails>(response);

                // Validate user is exist or not
                if (objUserDetails.Response == "User Exist")
                {
                    // Create xml which contain all user access pages
                    httpResponse = API.Post.PostObject("Account/GetAppFormListByUser", objUserDetails, "");
                    string responseAppForm = httpResponse.Content.ReadAsStringAsync().Result;
                    List<AppForm> appFormList = JsonConvert.DeserializeObject<List<AppForm>>(responseAppForm);
                    MenuGenerationHelper helperObj = new MenuGenerationHelper();
                    var fileName = Server.MapPath("~/Upload/" + objUserDetails.UserId + ".xml");

                    // Call method for generate XML
                    helperObj.GenerateXmlFile(appFormList, fileName);

                    // Store the login user details into session
                    Session["Authorize"] = objUserDetails;

                    return Redirect("/Home/Index");
                }
                else
                {
                    return Redirect("/Account/Login");
                }
            }
            return View();
        }

        /// <summary>
        /// Page for show access denied
        /// </summary>
        /// <returns></returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult LogOff() {
            Session.RemoveAll();
            //Redirect to login
            return Redirect("~/Account/Login");
        }
       
    }
}