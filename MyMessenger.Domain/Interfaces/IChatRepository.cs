using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        IQueryable<Chat> GetChatsByUserId(string userId);
        Task<Chat> GetChatById(int id);
        Task AddChat(string chatName, string ownerId, User user);
        void AddMember(Chat chat, User user);
        void DeleteMember(Chat chat, User user);
    }
}
