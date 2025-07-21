using MyMessenger.Maui.Models;
using Syncfusion.Blazor.Grids;
using MyMessenger.Maui.Services.SignalR;

namespace MyMessenger.Maui.Services.PageService
{
    public class ChatMessagePageService
    {
        private SignalRMessageService chatHubService;
        private MessageService messageService;

        public int ChatId { get; set; }
        public bool isWindowVisible = false;
        public bool showAddMenu = false;
        public bool showUpdateMenu = false;
        public string newMessage = "";
        public string newUpdateMessageText = "";
        MessageDTO currentMessage = new MessageDTO();
        

        public SfGrid<MessageDTO> grid;
        public List<MessageDTO> MessagesList = new List<MessageDTO>();
        public AllDataRetrievalParametersDTO CurrentPage;

        public event System.Action OnDataChanged;
        public ChatMessagePageService(SignalRMessageService chatHubService, MessageService messageService)
        {
            this.chatHubService = chatHubService;
            this.messageService = messageService;
            chatHubService.OnReceiveMessage += ReceiveMessage;
            chatHubService.OnReceiveUpdatedMessage += ReceiveUpdatedMessage;
        }
        public async Task EstablishConnection(string chatId)
        {
            ChatId = Convert.ToInt32(chatId);
            await chatHubService.InitializeHubConnection(chatId);
            await RetrieveData();
        }

        public void ReceiveMessage(MessageDTO receivedMessage)
        {
            MessagesList.Add(receivedMessage);
            grid.Refresh();
        }
        public void ReceiveUpdatedMessage(MessageDTO receivedMessage)
        {
            var message = MessagesList.First(m => m.Id == receivedMessage.Id);
            var index = MessagesList.IndexOf(message);
            MessagesList[index] = receivedMessage;
            grid.Refresh();
        }
        public async Task RetrieveData()
        {
            CurrentPage = new AllDataRetrievalParametersDTO() { PageNumber = 1, PageSize = 9999, Sort = new Dictionary<string, bool>(), Subs = "" };
            DataForGridDTO<MessageDTO> newData = await messageService.GetAll(CurrentPage, $"Message/{ChatId}");
            MessagesList = newData.Data.ToList();
            OnDataChanged?.Invoke();
        }
        public void ChangeAddMessageVisibility()
        {
            showAddMenu = !showAddMenu;
        }
        public async Task AddMessage()
        {
            if (!string.IsNullOrEmpty(newMessage))
            {
                await chatHubService.SendMessage(new MessageDTO() { ChatId = ChatId, Text = newMessage, DateTime = DateTime.Now });
                showAddMenu = false;
            }
            newMessage = "";
        }
        public void ShowUpdateMessage(MessageDTO message)
        {
            currentMessage = message;
            showUpdateMenu = true;
        }
        public async Task UpdateMessage()
        {
            currentMessage.Text = newUpdateMessageText;
            if (!string.IsNullOrEmpty(currentMessage.Text))
            {
                await chatHubService.UpdateMessage(currentMessage);
                showUpdateMenu = false;
            }
            currentMessage = new MessageDTO();
        }
        public void CloseUpdateMessage()
        {
            currentMessage = new MessageDTO();
            showUpdateMenu = false;
        }
    }
}
