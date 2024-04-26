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
        [Fact]
        public async Task GetAllChatsQueryHandler_ReturnsData()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var sort = new Dictionary<string, bool> { { "Param", true } };
            var query = new GetAllChatsQuery(sort, 1, 10, "", "UserId");
            var mockChats = new List<Chat>
            {
                new Chat { Id = 1, Name = "Chat 1" },
                new Chat { Id = 2, Name = "Chat 2" },
                new Chat { Id = 3, Name = "Chat 3" }
            }.AsQueryable();
            var mockQueryResult = new Dictionary<IEnumerable<Chat>, int> {{ mockChats, mockChats.Count() }};
            unitOfWorkMock.Setup(uow => uow.Chat.GetChatsByUserId(It.IsAny<string>())).Returns(mockChats);
            unitOfWorkMock.Setup(uow => uow.GetRepository<Chat>().FilterByQuery(It.IsAny<IQueryable<Chat>>(), sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(mockQueryResult));
            var expectedMappedResult = mockChats.Select(chat => new ChatDTO { Id = chat.Id, Name = chat.Name }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ChatDTO>>(It.IsAny<IEnumerable<Chat>>())).Returns(expectedMappedResult);
            var handler = new GetAllChatsQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
