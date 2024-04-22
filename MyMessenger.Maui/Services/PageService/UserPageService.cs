using MyMessenger.Application.DTO.UserDTOs;
using MyMessenger.Application.DTO;

namespace MyMessenger.Maui.Services.PageService
{
    public class UserPageService
    {
        private UserService userService;

        public IEnumerable<UserDTO> UsersList;
        public AllDataRetrievalParametersDTO CurrentPage;
        public List<string> Columns = new List<string> { "Name", "UserName", "Email", "PhoneNumber" };
        public bool isWindowVisible = false;
        public int numberOfPages;
        public int currentPage;
        public int pageSize;

        public event Action OnDataChanged;
        public UserPageService(UserService userService)
        {
            this.userService = userService;

            numberOfPages = 1;
            currentPage = 1;
            pageSize = 10;
            CurrentPage = new AllDataRetrievalParametersDTO() { PageNumber = currentPage, PageSize = pageSize, Sort = new Dictionary<string, bool>() { { "Name", false } }, Subs = "" };
            GetAllUsers();

        }
        public void ChangePage()
        {
            CurrentPage.PageNumber = currentPage;
            GetAllUsers();
        }
        public async void GetAllUsers()
        {
            DataForGridDTO<UserDTO> newdata = await userService.GetAll(CurrentPage, "User");
            UsersList = newdata.Data;
            numberOfPages = newdata.NumberOfPages;
            OnDataChanged?.Invoke();
        }

        // Parameters
        public void SaveChanges()
        {
            GetAllUsers();
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
