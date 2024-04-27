using Moq;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.СommandsQueries.Chats.Commands;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.Tests.Commands
{
    public class ChatCommandsTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IUserService> userServiceMock;
        public ChatCommandsTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            userServiceMock = new Mock<IUserService>();
        }
        [Fact]
        public async Task CreateChatCommandHandler_InvokesAddChat()
        {
            var OwnerId = "id";
            var ChatId = "id";
            var UserId = "id";
            var command = new CreateChatCommand(OwnerId, ChatId, "1");
            var sut = new CreateChatCommandHandler(unitOfWorkMock.Object, userServiceMock.Object);
            unitOfWorkMock.Setup(m => m.Chat.AddChat(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>())).Returns(Task.CompletedTask);

            await sut.Handle(command, default);

            unitOfWorkMock.Verify(m => m.Chat.AddChat(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()), Times.Once);
            unitOfWorkMock.Verify(m => m.SaveAsync(), Times.Once);
        }
        [Theory]
        [InlineData("Owner")]
        [InlineData("NotOwner")]
        public async Task DeleteChatCommandHandler_InvokesDeleteChat_IfUserIsOwner(string OwnerId)
        {
            int id = 1;
            var command = new DeleteChatCommand(OwnerId, id);
            var sut = new DeleteChatCommandHandler(unitOfWorkMock.Object);
            var chat = new Chat() { Id = id, OwnerId = "Owner" };
            unitOfWorkMock.Setup(m => m.GetRepository<Chat>().Delete(It.IsAny<Chat>()));
            unitOfWorkMock.Setup(m => m.GetRepository<Chat>().GetById(id)).Returns(Task.FromResult(chat));

            await sut.Handle(command, default);

            if (OwnerId == "Owner")
            {
                unitOfWorkMock.Verify(m => m.GetRepository<Chat>().Delete(chat), Times.Once);
                unitOfWorkMock.Verify(m => m.SaveAsync(), Times.Once);
            }
            else
            {
                unitOfWorkMock.Verify(m => m.GetRepository<Chat>().Delete(chat), Times.Never);
                unitOfWorkMock.Verify(m => m.SaveAsync(), Times.Never);
            }
        }
        [Fact]
        public async Task JoinChatCommandHandler_InvokesChat()
        {
            var UserName = "Name";
            var ChatId = 1;
            var command = new JoinChatCommand(UserName, ChatId);
            var sut = new JoinChatCommandHandler(unitOfWorkMock.Object, userServiceMock.Object);
            var chat = new Chat();
            unitOfWorkMock.Setup(m => m.GetRepository<Chat>().GetById(It.IsAny<int>())).Returns(Task.FromResult(chat));
            unitOfWorkMock.Setup(m => m.Chat.AddMember(It.IsAny<Chat>(), It.IsAny<User>()));

            await sut.Handle(command, default);

            unitOfWorkMock.Verify(m => m.Chat.AddMember(It.IsAny<Chat>(), It.IsAny<User>()), Times.Once);
            unitOfWorkMock.Verify(m => m.SaveAsync(), Times.Once);
        }
        [Theory]
        [InlineData("Owner")]
        [InlineData("NotOwner")]
        public async Task RemoveMemberFromChat_RemovesMemberOrMemberAndChat_IfUserIsOwner(string OwnerId)
        {
            var UserName = "Name";
            var ChatId = 1;
            var command = new LeaveChatCommand(UserName, ChatId);
            var sut = new LeaveChatCommandHandler(unitOfWorkMock.Object, userServiceMock.Object);
            var chat = new Chat() { OwnerId = "Owner" };
            var user = new User() { Id = OwnerId };
            userServiceMock.Setup(m => m.GetUserById(It.IsAny<string>())).Returns(Task.FromResult(user));
            unitOfWorkMock.Setup(m => m.GetRepository<Chat>().Delete(It.IsAny<Chat>()));
            unitOfWorkMock.Setup(m => m.Chat.GetChatById(It.IsAny<int>())).Returns(Task.FromResult(chat));
            unitOfWorkMock.Setup(m => m.Chat.DeleteMember(It.IsAny<Chat>(), It.IsAny<User>()));

            await sut.Handle(command, default);

            unitOfWorkMock.Verify(m => m.Chat.DeleteMember(It.IsAny<Chat>(), It.IsAny<User>()), Times.Once);
            unitOfWorkMock.Verify(m => m.SaveAsync(), Times.Once);
            if (OwnerId == "Owner")
            {
                unitOfWorkMock.Verify(m => m.GetRepository<Chat>().Delete(chat), Times.Once);
            }
        }
    }
}
