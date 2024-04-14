using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.СommandsQueries.Chats.Commands;
using MyMessenger.Application.СommandsQueries.Chats.Queries;
using MyMessenger.Application.СommandsQueries.Users.Queries;
using MyMessenger.Domain.Entities;

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
        public async Task CreateChat([FromBody] ChatDTO chat, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var user = await mediator.Send(new GetUserByIdQuery(userid));
            await mediator.Send(new CreateChatCommand(userid, chat.Name, user));
        }
        [HttpPost("member/")]
        public async Task JoinChat([FromBody] ChatDTO chat, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var user = await mediator.Send(new GetUserByIdQuery(userid));
            await mediator.Send(new JoinChatCommand(user, chat.Id));
        }
        [HttpDelete]
        public async Task DeleteChat([FromBody] ChatDTO chat, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            await mediator.Send(new DeleteChatCommand(userid, chat.Id));
        }
        [HttpDelete("member/")]
        public async Task LeaveChat([FromBody] ChatDTO chat, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var user = await mediator.Send(new GetUserByIdQuery(userid));
            await mediator.Send(new LeaveChatCommand(user, chat.Id));
        }
    }
}
