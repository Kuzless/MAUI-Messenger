using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        IQueryable<Chat> GetChatsByUserId(string userId);
        Chat GetChatByNameOwner(string chatName, string ownerId);
        Task AddChat(string chatName, string ownerId);
        Task DeleteChat(Chat chat);
        Task AddMember(Chat chat, User user);
        Task DeleteMember(Chat chat, User user);
    }
}
