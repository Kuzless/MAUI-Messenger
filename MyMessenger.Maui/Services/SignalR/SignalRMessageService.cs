using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR.Client;
using MyMessenger.Application.DTO.MessagesDTOs;

namespace MyMessenger.Maui.Services.SignalR
{
    public class SignalRMessageService
    {
        private readonly ILocalStorageService storage;
        private HubConnection hubConnection;
        public event Action<MessageDTO> OnReceiveMessage;
        public SignalRMessageService(ILocalStorageService storage)
        {
            this.storage = storage;
        }
        public async Task InitializeHubConnection(string chatId)
        {
            var token = (await storage.GetItemAsStringAsync("accessToken")).Replace("\"", "");
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7081/chathub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            }).Build();

            hubConnection.On<MessageDTO>("ReceiveMessage", async (receivedMessage) =>
            {
                OnReceiveMessage?.Invoke(receivedMessage);
            });
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("AddToGroup", chatId);
        }

        public async Task SendMessage(MessageDTO messageToSend)
        {
            await hubConnection.SendAsync("SendMessage", messageToSend);
        }
    }
}
