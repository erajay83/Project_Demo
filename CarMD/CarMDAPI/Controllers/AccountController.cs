using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CarMD.Shared.Models;
using DataAccessLayer.Service;

namespace CarMDAPI.Controllers
{
    public class AccountController : ApiController
    {
        public static string CONTENT_TYPE = @"application/x-www-form-urlencoded";
        public static string POST_METHOD = "POST";
        public static string GET_METHOD = "GET";
        public static string PUT_METHOD = "PUT";
        public static AccessTokenModel authToken;
        public static string physmodoAccessToken;
        AuthenticationService objAuthenticationService = new AuthenticationService();

        /// <summary>
        /// Call for login and authenticate user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Login(UserLoginModel model)
        {
            try
            {              
                UserDetails objUserDetails = new UserDetails();

                // Validate the user
                var response = objAuthenticationService.ValidateUserLogin(model);
                if (response.Response == "User Exist")
                {
                    // Generate Token
                    var token = GetToken(model.EmailAddress, model.Password);

                    response.Access_token = token.access_token;
                    response.Token_type = token.token_type;
                    response.Expires_in = token.expires_in;

                    return Ok(response);
                }
                else
                {
                    ErrorModel objErrorModel = new ErrorModel();
                    objErrorModel.ErrorCode = "404";
                    objErrorModel.ErrorMessage = response.Response;
                    return Ok(objErrorModel);
                }
            }catch(Exception ex)
            {
                // Get the exception code
                var w32ex = ex as Win32Exception;
                int code = 0;
                if (w32ex == null)
                {
                    w32ex = ex.InnerException as Win32Exception;
                }
                if (w32ex != null)
                {
                    code = w32ex.ErrorCode;
                    // do stuff
                }

                ErrorModel objErrorModel = new ErrorModel();
                objErrorModel.ErrorCode = code.ToString();
                objErrorModel.ErrorMessage = ex.Message;
                return Ok(objErrorModel);
            }
        }

        /// <summary>
        /// Build request for generate token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static AccessTokenModel GetToken(string userName, string password)
        {           
            var tokenUrl = ConfigurationManager.AppSettings["WebAPIURL"] + "/token";           
            var request = string.Format("grant_type=password&username={0}&password={1}", userName, password);

            // Request for generate token with required parameters
            authToken = HttpPost(tokenUrl, request);

            // Return the token
            return authToken;
        }

        /// <summary>
        /// Send request for generate token using Owin
        /// </summary>
        /// <param name="tokenUrl"></param>
        /// <param name="requestDetails"></param>
        /// <returns></returns>
        public static AccessTokenModel HttpPost(string tokenUrl, string requestDetails)
        {
            AccessTokenModel token = null;
            try
            {
                WebRequest webRequest = WebRequest.Create(tokenUrl);
                webRequest.ContentType = CONTENT_TYPE;
                webRequest.Method = POST_METHOD;
                byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
                webRequest.ContentLength = bytes.Length;
                using (Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    StreamReader newstreamreader = new StreamReader(webResponse.GetResponseStream());
                    string newresponsefromserver = newstreamreader.ReadToEnd();
                    newresponsefromserver = newresponsefromserver.Replace(".expires", "expires").Replace(".issued", "issued");
                    token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenModel>(newresponsefromserver);// new JavaScriptSerializer().Deserialize<AccessToken>(newresponsefromserver);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                token = null;
            }

            return token;
        }
        [HttpPost]
        public HttpResponseMessage GetAppFormListByUser(UserDetails model) {
            return Request.CreateResponse(HttpStatusCode.OK, objAuthenticationService.GetAppFormListByUser(model));
        }

        /// <summary>
        /// Create model for store token details
        /// </summary>
        public class AccessTokenModel
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string userName { get; set; }
            public string issued { get; set; }
            public string expires { get; set; }

        }
    }
}
