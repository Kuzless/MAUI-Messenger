namespace MyMessenger.Maui.Library.Interface
{
    public interface IHttpWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url, string token = "");
        Task<HttpResponseMessage> PostAsync(string url, string content, string token = "");
    }
}
