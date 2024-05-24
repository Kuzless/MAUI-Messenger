namespace MyMessenger.Maui.Library.Interface
{
    public interface IHttpWrapper
    {
        Task<HttpResponseMessage> GetAsync(string urlEnd, string token = "");
        Task<HttpResponseMessage> PostAsync(string urlEnd, string content, string token = "");
        Task<HttpResponseMessage> DeleteAsync(string urlEnd, string token = "");
        Task<HttpResponseMessage> PutAsync(string urlEnd, string content, string token = "");
        Task<HttpResponseMessage> PostImageAsync(string urlEnd, MultipartFormDataContent content, string token = "");
    }
}
