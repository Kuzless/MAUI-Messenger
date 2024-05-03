using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.DTO;
using MyMessenger.Application.Integration.Tests.Configuration;
using MyMessenger.Domain;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.UserDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyMessenger.Application.Integration.Tests.Controllets
{
    public class UserControllerTests : IClassFixture<MyMessengerWebApplicationFactory>
    {
        private readonly HttpClient client;
        private readonly DatabaseContext context;
        public UserControllerTests(MyMessengerWebApplicationFactory factory) : base()
        {
            client = factory.CreateClient();
            context = factory.databaseContext;
        }
        [Fact]
        public async Task GetAllUsers_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/User/");

            //Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetAllUsers_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/User/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllUsers_ReturnsChats()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/User/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }
        [Fact]
        public async Task GetAllUsers_ReturnsCorrectNumberOfChats()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/User/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count() == 2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAll_WhenSorted_ReturnsSortedResult(bool sortRule)
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);
            var data = new AllDataRetrievalParametersDTO() { Sort = new Dictionary<string, bool>() { { "Name", sortRule } } };
            var queryString = $"PageNumber={data.PageNumber}&PageSize={data.PageSize}";

            if (!string.IsNullOrEmpty(data.Subs))
            {
                queryString += $"&Subs={Uri.EscapeDataString(data.Subs)}";
            }

            if (data.Sort != null && data.Sort.Count > 0)
            {
                foreach (var (key, value) in data.Sort)
                {
                    queryString += $"&Sort[{Uri.EscapeDataString(key)}]={value}";
                }
            }

            //Act
            var response = await client.GetAsync($"/api/User?{queryString}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            if (sortRule)
            {
                Assert.True(result.Data.First().Name == "IntegrationTestsUser2");
            } else
            {
                Assert.True(result.Data.First().Name == "IntegrationTestsUser");
            }
            
        }
        [Theory]
        [InlineData("er2")]
        [InlineData("er")]
        [InlineData(".com")]
        public async Task GetAll_WhenSubstringSpecified_ReturnsCorrectRecords(string substring)
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);
            var data = new AllDataRetrievalParametersDTO() { Subs = substring };
            var queryString = $"PageNumber={data.PageNumber}&PageSize={data.PageSize}";

            if (!string.IsNullOrEmpty(data.Subs))
            {
                queryString += $"&Subs={Uri.EscapeDataString(data.Subs)}";
            }

            if (data.Sort != null && data.Sort.Count > 0)
            {
                foreach (var (key, value) in data.Sort)
                {
                    queryString += $"&Sort[{Uri.EscapeDataString(key)}]={value}";
                }
            }

            //Act
            var response = await client.GetAsync($"/api/User?{queryString}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            if (substring == "er2")
            {
                Assert.True(result.Data.Count() == 1);
            } else
            {
                Assert.True(result.Data.Count() == 2);
            }

        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAll_WhenPageSizeSpecified_ReturnsCorrectNumberOfRecords(int pageSize)
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);
            var data = new AllDataRetrievalParametersDTO() { PageSize = pageSize };
            var queryString = $"PageNumber={data.PageNumber}&PageSize={data.PageSize}";

            if (!string.IsNullOrEmpty(data.Subs))
            {
                queryString += $"&Subs={Uri.EscapeDataString(data.Subs)}";
            }

            if (data.Sort != null && data.Sort.Count > 0)
            {
                foreach (var (key, value) in data.Sort)
                {
                    queryString += $"&Sort[{Uri.EscapeDataString(key)}]={value}";
                }
            }

            //Act
            var response = await client.GetAsync($"/api/User?{queryString}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count() == pageSize);

        }
        private async Task<TokensDTO> Authorize()
        {
            var loginModel = new { Email = "IntegrationTestsUser@IntegrationTestsUser.com", Password = "123QWEasd!" };
            var tokenResponse = await client.PostAsJsonAsync("/api/Auth", loginModel);
            tokenResponse.EnsureSuccessStatusCode();
            var tokens = await tokenResponse.Content.ReadFromJsonAsync<TokensDTO>();
            return tokens;
        }
    }
}
