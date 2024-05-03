using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.ÑommandsQueries.Messages.Commands;
using MyMessenger.HubConfig;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyMessenger.Tests
{
    public class ChatHubTests
    {
        private readonly ChatHub hub;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IClientProxy> clientProxyMock;
        private readonly Mock<IHubCallerClients> clientsMock;
        private readonly Mock<IGroupManager> groupsMock;
        private readonly Mock<HubCallerContext> contextMock;

        public ChatHubTests()
        {
            mediatorMock = new Mock<IMediator>();
            clientsMock = new Mock<IHubCallerClients>();
            groupsMock = new Mock<IGroupManager>();
            clientProxyMock = new Mock<IClientProxy>();
            contextMock = new Mock<HubCallerContext>();

            clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);
            contextMock.Setup(c => c.ConnectionId).Returns("id");

            hub = new ChatHub(mediatorMock.Object)
            {
                Clients = clientsMock.Object,
                Groups = groupsMock.Object,
                Context = contextMock.Object,
            };
        }

        [Fact]
        public async Task AddToGroup_AddsConnectionToGroup()
        {
            // Arrange
            var chatId = "1";

            // Act
            await hub.AddToGroup(chatId);

            // Assert
            groupsMock.Verify(g => g.AddToGroupAsync(contextMock.Object.ConnectionId, chatId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}