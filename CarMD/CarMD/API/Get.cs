using System.Net.Http;

namespace CarMD.API
{
    public class Get
    {
        public static HttpResponseMessage APIRequestAll(string entityName, string userToken)
        {
            HttpClient client = Client.GetClient(userToken);
            var response = client.GetAsync(entityName).Result;
            return response;
        }
    }
}