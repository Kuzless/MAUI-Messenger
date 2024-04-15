using MyMessenger.Maui.Library.Interface;
using System.Text;

namespace MyMessenger.Maui.Library
{
    public class HttpWrapper : IHttpWrapper
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7081/api/";
        public HttpWrapper()
        {
            httpClient = new HttpClient();
        }
        public async Task<HttpResponseMessage> GetAsync(string urlEnd, string token = "")
        {
            AddHeaderToken(token);
            string urlController = url + urlEnd;
            HttpResponseMessage response = await httpClient.GetAsync(urlController);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string urlEnd, string content, string token = "")
        {
            AddHeaderToken(token);
            string urlController = url + urlEnd;
            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(urlController, httpContent);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> PutAsync(string urlEnd, string content, string token = "")
        {
            AddHeaderToken(token);
            string urlController = url + urlEnd;
            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync(urlController, httpContent);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string urlEnd, string token = "")
        {
            AddHeaderToken(token);
            string urlController = url + urlEnd;
            HttpResponseMessage response = await httpClient.DeleteAsync(urlController);
            response.EnsureSuccessStatusCode();
            return response;
        }
        private void AddHeaderToken(string token)
        {
            token = token.Replace("\"", "");
            if (!httpClient.DefaultRequestHeaders.Contains("userAccessToken"))
            {
                httpClient.DefaultRequestHeaders.Add("userAccessToken", token);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Remove("userAccessToken");
                httpClient.DefaultRequestHeaders.Add("userAccessToken", token);
            }
        }
    }
}
