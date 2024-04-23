using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Application.DTO;

namespace MyMessenger.Maui.Services.PageService
{
    public class ChatPageService
    {
        private ChatService chatService;
        public IEnumerable<ChatDTO> ChatsList;
        public AllDataRetrievalParametersDTO CurrentPage;
        public List<string> Columns { get; set; }
        public ChatDTO currentChat { get; set; }
        public bool isWindowVisible { get; set; }
        public bool showAddMenu { get; set; }
        public bool showInviteMenu { get; set; }
        public int numberOfPages { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public string newChatName { get; set; }
        public string userNameToInvite { get; set; }

        public event Action OnDataChanged;

        public ChatPageService(ChatService chatService)
        {
            this.chatService = chatService;

            Columns = new List<string> { "Id", "Name" };
            currentChat = new ChatDTO();
            isWindowVisible = false;
            showAddMenu = false;
            showInviteMenu = false;
            numberOfPages = 1;
            currentPage = 1;
            pageSize = 10;
            CurrentPage = new AllDataRetrievalParametersDTO() { PageNumber = currentPage, PageSize = pageSize, Sort = new Dictionary<string, bool>() { { "Name", false } }, Subs = "" };
            GetAllChats();

        }
        public void ChangePage()
        {
            CurrentPage.PageNumber = currentPage;
            GetAllChats();
        }

        public async void GetAllChats()
        {
            DataForGridDTO<ChatDTO> newdata = await chatService.GetAll(CurrentPage, "Chat");
            ChatsList = newdata.Data;
            numberOfPages = newdata.NumberOfPages;
            OnDataChanged?.Invoke();
        }

        public async void AddChat()
        {
            if (!string.IsNullOrEmpty(newChatName))
            {
                await chatService.AddChat(new ChatDTO() { Name = newChatName });
                showAddMenu = false;
                GetAllChats();
            }
        }

        public async void DeleteChat(int id)
        {
            await chatService.DeleteChat(id);
            GetAllChats();
        }
        public async void InviteToChat()
        {
            if (!string.IsNullOrEmpty(userNameToInvite))
            {
                await chatService.InviteToChat(currentChat, userNameToInvite);
                GetAllChats();
            }
            CloseInviteMenu();
            userNameToInvite = "";
        }
        public async void LeaveChat(int chatId)
        {
            await chatService.LeaveChat(chatId);
            GetAllChats();
        }
        public void ShowInviteToChat(ChatDTO chat)
        {
            currentChat = chat;
            showInviteMenu = true;
        }
        public void CloseInviteMenu()
        {
            currentChat = new ChatDTO();
            showInviteMenu = false;
        }
        public void ChangeParametersVisibility()
        {
            isWindowVisible = !isWindowVisible;
        }

        public void ChangeAddChatVisibility()
        {
            showAddMenu = !showAddMenu;
        }

        // Parameters
        public void SaveChanges()
        {
            GetAllChats();
        }
        public void OpenPopup()
        {
            isWindowVisible = true;
        }
        public void ClosePopup()
        {
            isWindowVisible = false;
        }
    }
}
