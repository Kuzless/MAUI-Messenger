using AutoMapper;
using Moq;
using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Application.СommandsQueries.Chats.Queries;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.Tests.Queries
{
    public class ChatQueriesTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        public ChatQueriesTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllChatsQueryHandler_ReturnsData()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllChatsQuery(sort, 1, 10, "", "UserId");
            var chats = new List<Chat>
            {
                new Chat { Id = 1, Name = "Chat 1", OwnerId = "UserId" },
                new Chat { Id = 2, Name = "Chat 2", OwnerId = "UserId" },
                new Chat { Id = 3, Name = "Chat 3", OwnerId = "UserId" }
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Chat>, int> { { chats, chats.Count() } };
            unitOfWorkMock.Setup(uow => uow.Chat.GetChatsByUserId(It.IsAny<string>())).Returns(chats);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Chat>().FilterByQuery(chats, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ChatDTO>>(It.IsAny<IEnumerable<Chat>>())).Returns(It.IsAny<IEnumerable<ChatDTO>>);
            var sut = new GetAllChatsQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllChatsQueryHandler_ReturnsCorrectNumberOfPages()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllChatsQuery(sort, 1, 10, "", "UserId");
            var chats = new List<Chat>
            {
                new Chat { Id = 1, Name = "Chat 1", OwnerId = "UserId" },
                new Chat { Id = 2, Name = "Chat 2", OwnerId = "UserId" },
                new Chat { Id = 3, Name = "Chat 3", OwnerId = "UserId" }
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Chat>, int> { { chats, chats.Count() } };
            unitOfWorkMock.Setup(uow => uow.Chat.GetChatsByUserId(It.IsAny<string>())).Returns(chats);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Chat>().FilterByQuery(chats, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = chats.Select(chat => new ChatDTO { Id = chat.Id, Name = chat.Name }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ChatDTO>>(It.IsAny<IEnumerable<Chat>>())).Returns(mappedResults);
            var sut = new GetAllChatsQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.NumberOfPages);
        }
        [Fact]
        public async Task GetAllChatsQueryHandler_ReturnsCorrectMappedValues()
        {
            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllChatsQuery(sort, 1, 10, "", "UserId");
            var chats = new List<Chat>
            {
                new Chat { Id = 1, Name = "Chat 1", OwnerId = "UserId" },
                new Chat { Id = 2, Name = "Chat 2", OwnerId = "UserId" },
                new Chat { Id = 3, Name = "Chat 3", OwnerId = "UserId" }
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<Chat>, int> { { chats, chats.Count() } };
            unitOfWorkMock.Setup(uow => uow.Chat.GetChatsByUserId(It.IsAny<string>())).Returns(chats);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Chat>().FilterByQuery(chats, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = chats.Select(chat => new ChatDTO { Id = chat.Id, Name = chat.Name }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ChatDTO>>(It.IsAny<IEnumerable<Chat>>())).Returns(mappedResults);
            var sut = new GetAllChatsQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(chats.Count(), result.Data.Count());
        }
    }
}
