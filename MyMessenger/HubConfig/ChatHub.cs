using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using System.Security.Claims;

namespace MyMessenger.HubConfig
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;
        public ChatHub(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task AddToGroup(string ChatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatId);
        }
        public async Task SendMessage(MessageDTO message)
        {
            var userId = Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var name = Context.GetHttpContext().User.FindFirst("Name").Value;

            var messageId = await mediator.Send(new CreateMessageCommand(userId, message.ChatId, message.Text, message.DateTime));

            message.Name = name;
            message.Id = messageId;
            await Clients.Group(Convert.ToString(message.ChatId)).SendAsync("ReceiveMessage", message);
        }
        public async Task UpdateMessage(MessageDTO message)
        {
            var userId = Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await mediator.Send(new UpdateMessageCommand(message, userId));
            await Clients.Group(Convert.ToString(message.ChatId)).SendAsync("ReceiveUpdatedMessage", message);
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }

        public async Task Send(string message)
        {
            Console.WriteLine(message);
            await Clients.Others.SendAsync("Receive", message);
        }
    }
}
