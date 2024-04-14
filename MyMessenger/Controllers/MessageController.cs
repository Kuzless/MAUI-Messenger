using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.СommandsQueries.Messages.Queries;

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
            var userid = jWTRetrievalService.GetIdByToken(new TokensDTO() { accessToken = userAccessToken });
            var messages = await mediator.Send(new GetAllMessagesQuery(data.Sort, data.PageNumber, data.PageSize, data.Subs, userid));
            return Ok(messages);
        }
       /* public async Task UpdateMessage()
        {
            throw new NotImplementedException();
        }
        public async Task SendMessage()
        {
            throw new NotImplementedException();
        }
        public async Task DeleteMessage(int chatId, int messageId, int userId)
        {
            throw new NotImplementedException();
        }*/
    }
}
