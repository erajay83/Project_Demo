using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;


namespace CarMD.API
{
    public static class Client
    {

        public static Uri getAPIBaseAddress()
        {
            string APIVersion = ConfigurationManager.AppSettings["APIVersion"];
            string uriString = String.Empty;
            switch (APIVersion.ToUpper())
            {
                case "DEV":
                    uriString = "http://localhost:55337/api/";
                    break;
                case "Live":
                    uriString = "http://domain/api/";
                    break;
            }

            return new Uri(uriString);
        }
        public static HttpClient GetClient(string userkey)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = getAPIBaseAddress();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.API_INIT.JSON_HEADER));
            client.DefaultRequestHeaders.Add(Constants.API_INIT.USER_TOKEN, userkey);
            if (!string.IsNullOrEmpty(userkey))
                client.DefaultRequestHeaders.Add("Authorization", userkey);
            
            return client;
        }
    }
}