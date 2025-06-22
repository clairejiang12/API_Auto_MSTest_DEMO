using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestMethodLibrary
{
    public class HttpResult
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string? ResponseBody { get; set; } 
    }

    public class RequestMethodClass
    {
        private static string endpointUrl = "http://localhost:5089/";

        public static async Task<HttpResult> GetAsync(HttpClient httpClient, string url, bool isSuccess)
        {
            var response = await httpClient.GetAsync(endpointUrl + url);
            if (isSuccess)
            {
                response.EnsureSuccessStatusCode();
            }
            var body = await response.Content.ReadAsStringAsync();
            return new HttpResult
            {
                StatusCode = response.StatusCode,
                ResponseBody = body
            };
        }

        public static async Task<HttpResult> PostAsync(HttpClient httpClient, string url, string payload, bool isSuccess)
        {
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpointUrl+url, content);
            if (isSuccess)
            {
                response.EnsureSuccessStatusCode();
            }
            var body = await response.Content.ReadAsStringAsync();
            return new HttpResult
            {
                StatusCode = response.StatusCode,
                ResponseBody = body
            };
        }

        public static async Task<HttpResult> PutAsync(HttpClient httpClient, string url, string payload, bool isSuccess)
        {
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(endpointUrl+url, content);
            if (isSuccess)
            {
                response.EnsureSuccessStatusCode();
            }
            var body = await response.Content.ReadAsStringAsync();
            return new HttpResult
            {
                StatusCode = response.StatusCode,
                ResponseBody = body
            };
        }

        public static async Task<HttpResult> DeleteAsync(HttpClient httpClient, string url, bool isSuccess)
        {

            var response = await httpClient.DeleteAsync(endpointUrl+url);
            if (isSuccess)
            {
                response.EnsureSuccessStatusCode();
            }
            var body = await response.Content.ReadAsStringAsync();
            return new HttpResult
            {
                StatusCode = response.StatusCode,
                ResponseBody = body
            };
        }
    }
}