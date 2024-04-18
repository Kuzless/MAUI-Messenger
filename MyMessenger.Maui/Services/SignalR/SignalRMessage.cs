using Microsoft.AspNetCore.SignalR.Client;
using MyMessenger.Application.DTO.MessagesDTOs;

namespace MyMessenger.Maui.Services.SignalR
{
    public class SignalRMessage
    {
        private readonly MessageService messageService;
        private HubConnection hubConnection;
        private List<MessageDTO> MessagesList;
        public SignalRMessage(MessageService messageService)
        {
            this.messageService = messageService;
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7081/chathub").Build();

            hubConnection.On<MessageDTO>("ReceiveMessage", async (receivedMessage) =>
            {
                MessagesList.Add(receivedMessage);
            });
        }
        public async Task<MessageDTO> SendMessage(MessageDTO messageToSend)
        {
            await messageService.AddMessage(messageToSend);
            await hubConnection.StartAsync();
            await hubConnection.SendAsync("SendMessage", messageToSend);
            return messageToSend;
        }
    }
}
