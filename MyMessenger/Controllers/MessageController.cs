using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using MyMessenger.Application.СommandsQueries.Messages.Queries;
using MyMessenger.Application.СommandsQueries.Users.Queries;
using System.Security.Claims;

namespace MyMessenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMediator mediator;
        private readonly IJWTRetrievalService jWTRetrievalService;
        public MessageController(IMediator mediator, IJWTRetrievalService jWTRetrievalService)
        {
            this.jWTRetrievalService = jWTRetrievalService;
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMessages([FromQuery] AllDataRetrievalParametersDTO data, [FromHeader] string userAccessToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var messages = await mediator.Send(new GetAllMessagesQuery(data.Sort, data.PageNumber, data.PageSize, data.Subs, userid));
            return Ok(messages);
        }
        [HttpPut]
        public async Task UpdateMessage([FromBody] MessageDTO message, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            await mediator.Send(new UpdateMessageCommand(message, userid));
        }
        [HttpPost]
        public async Task SendMessage([FromBody] MessageDTO message, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            await mediator.Send(new CreateMessageCommand(message.Id, userid, message.ChatId, message.Text));
        }
        [HttpDelete]
        public async Task DeleteMessage([FromBody] MessageDTO message, [FromHeader] string userAccessToken)
        {
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            await mediator.Send(new DeleteMessageCommand(message.Id, userid));
        }
    }
}
