using MyMessenger.Maui.Models;

namespace MyMessenger.Maui.Services.PageService
{
    public class MessagePageService
    {
        private MessageService messageService;

        public IEnumerable<MessageDTO> MessagesList;
        public AllDataRetrievalParametersDTO CurrentPage;
        public List<string> Columns = new List<string> { "Text", "DateTime" };
        public bool isWindowVisible = false;
        public int numberOfPages;
        public int currentPage;
        public int pageSize;
        public event Action OnDataChanged;
        public MessagePageService(MessageService messageService)
        {
            this.messageService = messageService;
            numberOfPages = 1;
            currentPage = 1;
            pageSize = 10;
            CurrentPage = new AllDataRetrievalParametersDTO() { PageNumber = currentPage, PageSize = pageSize, Sort = new Dictionary<string, bool>() { { "Text", false } }, Subs = "" };
            GetAllMessages();
        }
        public void ChangePage()
        {
            CurrentPage.PageNumber = currentPage;
            GetAllMessages();
        }
        public async void GetAllMessages()
        {
            DataForGridDTO<MessageDTO> newdata = await messageService.GetAll(CurrentPage, "Message/0");
            MessagesList = newdata.Data;
            numberOfPages = newdata.NumberOfPages;
            OnDataChanged?.Invoke();
        }
        public void ChangeParametersVisibility()
        {
            isWindowVisible = !isWindowVisible;
        }
        public async void DeleteMessage(MessageDTO message)
        {
            await messageService.DeleteMessage(message.Id);
            GetAllMessages();
        }

        //Parameters
        public void SaveChanges()
        {
            GetAllMessages();
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
