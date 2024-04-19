using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using System.Security.Claims;

namespace MyMessenger.Options.HubConfig
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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
            await mediator.Send(new CreateMessageCommand(userId, message.ChatId, message.Text, message.DateTime));
            await Clients.Group(Convert.ToString(message.ChatId)).SendAsync("ReceiveMessage", message);
        }
    }
}
