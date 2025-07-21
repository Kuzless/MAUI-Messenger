using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using System.Security.Claims;

namespace MyMessenger.HubConfig
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;
        private static readonly Dictionary<string, HashSet<string>> SharedIdToConnections = new();

        public ChatHub(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task RegisterSharedId(string sharedId)
        {
            lock (SharedIdToConnections)
            {
                if (!SharedIdToConnections.ContainsKey(sharedId))
                    SharedIdToConnections[sharedId] = new HashSet<string>();
                SharedIdToConnections[sharedId].Add(Context.ConnectionId);
            }
            return Task.CompletedTask;
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            lock (SharedIdToConnections)
            {
                foreach (var set in SharedIdToConnections.Values)
                    set.Remove(Context.ConnectionId);
            }
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
        public async Task SendToSharedId(string sharedId, string message)
        {
            HashSet<string> targets;
            lock (SharedIdToConnections)
            {
                if (!SharedIdToConnections.TryGetValue(sharedId, out targets))
                    return;
                targets = new HashSet<string>(targets);
            }
            foreach (var connId in targets)
            {
                if (connId != Context.ConnectionId)
                    await Clients.Client(connId).SendAsync("Receive", message);
            }
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
    }
}
