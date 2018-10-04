using CarMD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CarMD.Auth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        // Create variable
        private readonly string[] allowedPage;
       
        public AuthorizeUser(params string[] page)        
        {
            this.allowedPage = page;
        }

        /// <summary>
        /// Authorize form access based on role management
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            bool authorize = false;
            UserDetails objUserDetails = (UserDetails)filterContext.HttpContext.Session["Authorize"];
            if (objUserDetails != null)
            {
              
                if (allowedPage.Count() > 0)
                {
                    bool flag = true;
                    foreach (var page in allowedPage)
                    {
                        if (ReadXML(page, objUserDetails.UserId))
                        {
                            authorize = true;
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                    }
                }
                else
                {
                    authorize = true;                   
                }
            }
            else
            {
                filterContext.HttpContext.Session.RemoveAll();
                //Redirect to login
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }

        /// <summary>
        /// Used to read the xml for check the user have authorize for access this page or not
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Boolean ReadXML(string actionName, string fileName)
        {            
            string xmlData =  HttpContext.Current.Server.MapPath("~/Upload/"+ fileName + ".xml");//Path of the xml script  
            var doc = new XmlDocument();
            doc.Load(xmlData);
            var root = doc.DocumentElement;
            if (root == null)
                return false;

            var Forms = root.SelectNodes("Form");
            if (Forms == null)
                return false;

            foreach (XmlNode item in Forms)
            {
               var formName = item.SelectSingleNode("FormName").InnerText;
                if (formName == actionName)
                {
                    return true;
                }
            }
            return false;

            
        }
    }    
}