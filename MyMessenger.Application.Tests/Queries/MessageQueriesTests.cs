using AutoMapper;
using Moq;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Queries;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;


namespace MyMessenger.Application.Tests.Queries
{
    public class MessageQueriesTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        public MessageQueriesTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetMessagesByChatIdQueryHandler_ReturnsData()
        {
            var query = new GetMessagesByChatIdQuery(1, 10, "", 1);
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.Message.GetMessagesByChatId(It.IsAny<int>())).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), It.IsAny<Dictionary<string, bool>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(It.IsAny<IEnumerable<MessageDTO>>);
            var sut = new GetMessagesByChatIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMessagesByChatIdQueryHandler_ReturnsCorrectNumberOfPages()
        {
            var query = new GetMessagesByChatIdQuery(1, 10, "", 1);
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.Message.GetMessagesByChatId(It.IsAny<int>())).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), It.IsAny<Dictionary<string, bool>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = messages.Select(chat => new MessageDTO { ChatId = chat.ChatId, Text = chat.Text }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(mappedResults);
            var sut = new GetMessagesByChatIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.NumberOfPages);
        }
        [Fact]
        public async Task GetMessagesByChatIdQueryHandler_ReturnsCorrectMappedValues()
        {
            var query = new GetMessagesByChatIdQuery(1, 10, "", 1);
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.Message.GetMessagesByChatId(It.IsAny<int>())).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), It.IsAny<Dictionary<string, bool>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = messages.Select(chat => new MessageDTO { ChatId = chat.ChatId, Text = chat.Text }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(mappedResults);
            var sut = new GetMessagesByChatIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(messages.Count(), result.Data.Count());
            foreach (var dto in result.Data)
            {
                Assert.Equal(1, dto.ChatId);
            }
        }
        [Fact]
        public async Task GetAllMessagesQueryHandler_ReturnsData()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllMessagesQuery(sort, 1, 10, "", "User1");
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetAll()).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(It.IsAny<IEnumerable<MessageDTO>>);
            var sut = new GetAllMessagesQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllMessagesQueryHandler_ReturnsCorrectNumberOfPages()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllMessagesQuery(sort, 1, 10, "", "User1");
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetAll()).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = messages.Select(chat => new MessageDTO { ChatId = chat.ChatId, Text = chat.Text }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(mappedResults);
            var sut = new GetAllMessagesQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.NumberOfPages);
        }
        [Fact]
        public async Task GetAllMessagesQueryHandler_ReturnsCorrectMappedValues()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllMessagesQuery(sort, 1, 10, "", "User1");
            var messages = new List<Message>
            {
                new Message { Id = 1 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
                new Message { Id = 2 , ChatId = 1, Text = "Hello", UserId = "User2", DateTime = DateTime.Now },
                new Message { Id = 3 , ChatId = 1, Text = "Hello", UserId = "User1", DateTime = DateTime.Now },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Message>, int> { { messages, messages.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().GetAll()).Returns(messages);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Message>().FilterByQuery(It.IsAny<IQueryable<Message>>(), sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = messages.Select(chat => new MessageDTO { ChatId = chat.ChatId, Text = chat.Text }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<MessageDTO>>(It.IsAny<IEnumerable<Message>>())).Returns(mappedResults);
            var sut = new GetAllMessagesQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(messages.Count(), result.Data.Count());
            foreach (var dto in result.Data)
            {
                Assert.Equal(1, dto.ChatId);
            }
        }
        [Fact]
        public async Task GetMessageByIdQueryHandler_ReturnsData()
        {
            var messageId = 1;
            var message = new Message { Id = messageId, Text = "Hello" };
            var messageDto = new MessageDTO { Id = messageId, Text = "Hello" };
            unitOfWorkMock.Setup(uow => uow.Message.GetMessageById(messageId)).ReturnsAsync(message);
            mapperMock.Setup(mapper => mapper.Map<MessageDTO>(message)).Returns(messageDto);
            var query = new GetMessageByIdQuery(messageId);
            var sut = new GetMessageByIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMessageByIdQueryHandler_ReturnsCorrectMessageDTO()
        {
            var messageId = 1;
            var message = new Message { Id = messageId, Text = "Hello" };
            var messageDto = new MessageDTO { Id = messageId, Text = "Hello" };
            unitOfWorkMock.Setup(uow => uow.Message.GetMessageById(messageId)).ReturnsAsync(message);
            mapperMock.Setup(mapper => mapper.Map<MessageDTO>(message)).Returns(messageDto);
            var query = new GetMessageByIdQuery(messageId);
            var sut = new GetMessageByIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(messageDto.Id, result.Id);
            Assert.Equal(messageDto.Text, result.Text);
        }
    }
}
