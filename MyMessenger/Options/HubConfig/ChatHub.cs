using MediatR;
using Microsoft.AspNetCore.SignalR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.СommandsQueries.Messages.Commands;

namespace MyMessenger.Options.HubConfig
{
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;
        private readonly IJWTRetrievalService jWTRetrievalService;
        public ChatHub(IMediator mediator, IJWTRetrievalService jWTRetrievalService)
        {
            this.jWTRetrievalService = jWTRetrievalService;
            this.mediator = mediator;
        }
        public async Task SendMessage(MessageDTO message)
        {
            var userid = "dd63f11c-c322-45ba-8e21-55271e183ff3";
            await mediator.Send(new CreateMessageCommand(userid, message.ChatId, message.Text));
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
