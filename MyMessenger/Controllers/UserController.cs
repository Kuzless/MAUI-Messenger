using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Application.DTO;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.СommandsQueries.Users.Commands;
using MyMessenger.Application.СommandsQueries.Users.Queries;
using System.Security.Claims;

namespace MyMessenger.Controllers
{
    [DisableCors]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator mediator;
        private readonly IBlobStorageService blob;
        public UserController(IMediator mediator, IBlobStorageService blob)
        {
            this.mediator = mediator;
            this.blob = blob;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] AllDataRetrievalParametersDTO data)
        {
            var users = await mediator.Send(new GetAllUsersQuery(data.Sort, data.PageNumber, data.PageSize, data.Subs));
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile image)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string? imageUrl;
            using (var stream = image.OpenReadStream())
            {
                imageUrl = await blob.UploadImageAsync(stream, $"{userid}", image.FileName);
            }
            await mediator.Send(new UpdateImageCommand(userid, imageUrl));
            return Ok(imageUrl);
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await mediator.Send(new GetUserDTOByIdQuery(userId));
            return Ok(user);
        }
        [HttpPut("{name}/{phonenumber}")]
        public async Task<IActionResult> UpdateInfo(string name, string phonenumber)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await mediator.Send(new UpdateInfoCommand(userid, name, phonenumber));
            return Ok();
        }
    }
}
