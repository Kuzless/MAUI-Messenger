using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Domain.Interfaces;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.JwtAuth.Interfaces;
using MyMessenger.MApplication.СommandsQueries.Chats.Queries;
using MyMessenger.MApplication.СommandsQueries.Messages.Queries;

namespace MyMessenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IMediator mediator;
        private readonly IJWTRetrievalService jWTRetrievalService;
        public ChatController(IMediator mediator, IJWTRetrievalService jWTRetrievalService)
        {
            this.jWTRetrievalService = jWTRetrievalService;
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChats([FromQuery] AllDataRetrievalParametersDTO data, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var chats = await mediator.Send(new GetAllChatsQuery(data.Sort, data.PageNumber, data.PageSize, data.Subs, userid));
            return Ok(chats);
        }
        [HttpPost]
        public async Task CreateChat()
        {
            throw new NotImplementedException();
        }
       [HttpDelete]
        public async Task DeleteChat()
        {
            throw new NotImplementedException();
        }
    }
}
