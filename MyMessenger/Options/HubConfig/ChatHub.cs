using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using MyMessenger.Application.СommandsQueries.Messages.Queries;

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
            var userid = "dd63f11c-c322-45ba-8e21-55271e183ff3";
            var messageId = await mediator.Send(new CreateMessageCommand(userid, message.ChatId, message.Text));
            var newMessage = await mediator.Send(new GetMessageByIdQuery(messageId));
            await Clients.All.SendAsync("ReceiveMessage", newMessage);
        }
    }
}
