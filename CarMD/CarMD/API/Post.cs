using System;
using System.Net.Http;

namespace CarMD.API
{
    public static class Post
    {
        public static HttpResponseMessage PostObject(string entityName, object model, string userkey)
        {
            HttpClient client = Client.GetClient(userkey);
            client.Timeout = new TimeSpan(0, 10, 0);
            var response = client.PostAsJsonAsync(entityName, model).Result;
            return response;
        }
    }
}