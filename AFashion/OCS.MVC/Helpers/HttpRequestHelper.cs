using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OCS.MVC.Helpers
{
    public static class HttpRequestHelper
    {
        private static string ServerAddr => ConfigurationManager.AppSettings["base-url"];

        private static HttpClient GetClient()
        {
            HttpClient HttpClient = new HttpClient();

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var principal= HttpContext.Current.User;
            if (principal.ClaimExists("AccessToken"))
            {
                string access_string = principal.GetClaim("AccessToken");
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",access_string);
            }

            return HttpClient;
        }

        public static async Task<HttpResponseMessage> GetAsync(string url="", string urlParam = "")
        {
            HttpResponseMessage response;

            using (HttpClient client = GetClient())
            {

                response = (urlParam.Length > 0) ? await client.GetAsync($"{ServerAddr}{url}?urlParams={urlParam}") :
                                                   await client.GetAsync($"{ServerAddr}{url}");
            }
            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync(string url, Object data)
        {
            HttpResponseMessage response;
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            using (HttpClient client = GetClient())
            {
                response = await client.PostAsync($"{ServerAddr}{url}", content);
            }
            return response;
        }
    }
}