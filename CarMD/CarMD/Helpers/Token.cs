using CarMD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMD.Helpers
{
    public class Token
    {
        public string GetAPIToken()
        {
            UserDetails objUserDetails = (UserDetails)HttpContext.Current.Session["Authorize"];
            if (objUserDetails != null)
            {
                var token = "Bearer " + objUserDetails.Access_token;
                return token;
            }
            else
            {
                return "No Token";
            }
        }
    }
}