using Azure;
using Moq;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Chats.Commands;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.Tests.Commands
{
    public class MessageCommandsTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        public MessageCommandsTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
        }
        [Fact]
        public async Task CreateMessageCommandHandler_InvokesAddMessage()
        {
            var userId = "UserId";
            var chatId = 1;
            var text = "message";
            var dateTime = DateTime.Now;
            var message = new Message() { UserId = userId, ChatId = chatId, Text = text, DateTime = dateTime };
            var command = new CreateMessageCommand(userId, chatId, text, dateTime);
            var sut = new CreateMessageCommandHandler(unitOfWorkMock.Object);
            unitOfWorkMock.Setup(uow => uow.Message.AddMessage(It.IsAny<Message>())).Returns(Task.FromResult(message));

            await sut.Handle(command, default);

            unitOfWorkMock.Verify(uow => uow.Message.AddMessage(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task CreateMessageCommandHandler_ReturnsCorrectValue()
        {
            var userId = "UserId";
            var chatId = 1;
            var text = "message";
            var dateTime = DateTime.Now;
            var message = new Message() { UserId = userId, ChatId = chatId, Text = text, DateTime = dateTime };
            var command = new CreateMessageCommand(userId, chatId, text, dateTime);
            var sut = new CreateMessageCommandHandler(unitOfWorkMock.Object);
            unitOfWorkMock.Setup(uow => uow.Message.AddMessage(It.IsAny<Message>())).Returns(Task.FromResult(message));

            var result = await sut.Handle(command, default);

            Assert.Equal(message.Id, result);
        }
        [Fact]
        public async Task DeleteMessageCommandHandler_InvokesDeleteMessage()
        {
            var userId = "UserId";
            var chatId = 1;
            var messageId = 1;
            var text = "message";
            var dateTime = DateTime.Now;
            var message = new Message() { UserId = userId, ChatId = chatId, Text = text, DateTime = dateTime };
            var command = new DeleteMessageCommand(messageId, userId);
            var sut = new DeleteMessageCommandHandler(unitOfWorkMock.Object);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetById(messageId)).Returns(Task.FromResult(message));
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().Delete(It.IsAny<Message>()));

            await sut.Handle(command, default);

            unitOfWorkMock.Verify(uow => uow.GetRepository<Message>().GetById(messageId), Times.Once);
            unitOfWorkMock.Verify(uow => uow.GetRepository<Message>().Delete(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteMessageCommandHandler_DeletesMessage()
        {
            var userId = "UserId";
            var messageId = 1;
            var message = new Message { UserId = userId, Id = messageId, Text = "Message to delete" };
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetById(messageId)).Returns(Task.FromResult(message));
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().Delete(It.IsAny<Message>()));
            var sut = new DeleteMessageCommandHandler(unitOfWorkMock.Object);
            var command = new DeleteMessageCommand(messageId, userId);

            var response = await sut.Handle(command, default);

            Assert.True(response.isSuccessful);
        }

        [Fact]
        public async Task DeleteMessageCommandHandler_ReturnsFailure_WhenUserIsNotOwner()
        {
            var userId = "UserId";
            var messageId = 1;
            var message = new Message { UserId = "UserId2", Id = messageId, Text = "Message to delete" };
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetById(messageId)).Returns(Task.FromResult(message));
            var sut = new DeleteMessageCommandHandler(unitOfWorkMock.Object);
            var command = new DeleteMessageCommand(messageId, userId);

            var response = await sut.Handle(command, default);

            Assert.False(response.isSuccessful);
        }
        [Fact]
        public async Task UpdateMessageCommandHandler_InvokesUpdateMessage()
        {
            var userId = "UserId";
            var messageId = 1;
            var updatedMessage = new MessageDTO { Id = messageId };
            var oldmessage = new Message { UserId = userId, Id = messageId };
            unitOfWorkMock.Setup(uow => uow.Message.GetById(messageId)).Returns(Task.FromResult(oldmessage));
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().Update(It.IsAny<Message>()));
            var sut = new UpdateMessageCommandHandler(unitOfWorkMock.Object);
            var command = new UpdateMessageCommand(updatedMessage, userId);

            var response = await sut.Handle(command, default);

            unitOfWorkMock.Verify(uow => uow.Message.GetById(messageId), Times.Once);
            unitOfWorkMock.Verify(uow => uow.GetRepository<Message>().Update(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateMessageCommandHandler_UpdatesMessage()
        {
            var userId = "UserId";
            var messageId = 1;
            var newText = "new message";
            var updatedMessage = new MessageDTO { Id = messageId, Text = newText };
            var oldmessage = new Message { UserId = userId, Id = messageId, Text = "old message" };
            unitOfWorkMock.Setup(uow => uow.Message.GetById(messageId)).Returns(Task.FromResult(oldmessage));
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().Update(It.IsAny<Message>()));
            var sut = new UpdateMessageCommandHandler(unitOfWorkMock.Object);
            var command = new UpdateMessageCommand(updatedMessage, userId);

            var response = await sut.Handle(command, default);

            Assert.True(response.isSuccessful);
            Assert.Equal(newText, oldmessage.Text);
        }
        [Fact]
        public async Task UpdateMessageCommandHandler_ReturnsFailure_WhenUserIsNotOwner()
        {
            var userId = "UserId";
            var messageId = 1;
            var newText = "new message";
            var updatedMessage = new MessageDTO { Id = messageId, Text = newText };
            var oldmessage = new Message { UserId = "UserId2", Id = messageId, Text = "old message" };
            unitOfWorkMock.Setup(uow => uow.Message.GetById(messageId)).Returns(Task.FromResult(oldmessage));
            var sut = new UpdateMessageCommandHandler(unitOfWorkMock.Object);
            var command = new UpdateMessageCommand(updatedMessage, userId);

            var response = await sut.Handle(command, default);

            Assert.False(response.isSuccessful);
            Assert.NotEqual(newText, oldmessage.Text);
        }
    }
}
