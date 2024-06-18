using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.СommandsQueries.Messages.Commands;
using MyMessenger.Application.СommandsQueries.Messages.Queries;
using System.Security.Claims;

namespace MyMessenger.Controllers
{
    [DisableCors]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMediator mediator;
        public MessageController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(int chatId, [FromQuery] AllDataRetrievalParametersDTO data)
        {
            if (chatId == 0)
            {
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var messages = await mediator.Send(new GetAllMessagesQuery(data.Sort, data.PageNumber, data.PageSize, data.Subs, userid));
                return Ok(messages);
            } else
            {
                var messages = await mediator.Send(new GetMessagesByChatIdQuery(data.PageNumber, data.PageSize, data.Subs, chatId));
                return Ok(messages);
            }
        }
        [HttpPut]
        public async Task UpdateMessage([FromBody] MessageDTO message)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await mediator.Send(new UpdateMessageCommand(message, userid));
        }
        [HttpPost]
        public async Task SendMessage([FromBody] MessageDTO message)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await mediator.Send(new CreateMessageCommand(userid, message.ChatId, message.Text, message.DateTime));
        }
        [HttpDelete("{id}")]
        public async Task DeleteMessage(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await mediator.Send(new DeleteMessageCommand(id, userid));
        }
    }
}
