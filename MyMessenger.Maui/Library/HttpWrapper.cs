using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Headers;
using System.Text;

namespace MyMessenger.Maui.Library
{
    public class HttpWrapper : IHttpWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly string url = "https://localhost:7081/api/";
        public HttpWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetAsync(string urlEnd, string token = "")
        {
            token = token.Replace("\"", "");
            string urlController = url + urlEnd;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(urlController);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> PostAsync(string urlEnd, string content, string token = "")
        {
            token = token.Replace("\"", "");
            string urlController = url + urlEnd;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(urlController, httpContent);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> PutAsync(string urlEnd, string content, string token = "")
        {
            token = token.Replace("\"", "");
            string urlController = url + urlEnd;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(urlController, httpContent);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string urlEnd, string token = "")
        {
            token = token.Replace("\"", "");
            string urlController = url + urlEnd;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync(urlController);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> PostImageAsync(string urlEnd, MultipartFormDataContent content, string token = "")
        {
            token = token.Replace("\"", "");
            string urlController = url + urlEnd;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsync(urlController, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
