using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        IQueryable<Chat> GetChatsByUserId(string userId);
        Chat GetChatById(int id);
        Task AddChat(string chatName, string ownerId, User user);
        Task AddMember(Chat chat, User user);
        Task DeleteMember(Chat chat, User user);
    }
}
