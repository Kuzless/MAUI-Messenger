using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services;
using MyMessenger.MApplication.Services.Interfaces;
using MyMessenger.MApplication.СommandsQueries.Users.Commands;
using MyMessenger.MApplication.СommandsQueries.Users.Queries;

namespace MyMessenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<TokensDTO>> Login([FromBody] LoginDTO user)
        {
            var tokens = await mediator.Send(new LoginQuery(user));
            Console.WriteLine($"!!!\n{tokens.accessToken}\n!!!");
            Console.WriteLine($"!!!\n{tokens.refreshToken}\n!!!");
            if (tokens.accessToken != null)
            {
                return Ok(tokens);
            } else
            {
                return BadRequest("Invalid login credentials");
            }
        }
        [HttpPost("refresh/")]
        public async Task<ActionResult<TokensDTO>> Refresh([FromBody] TokensDTO tokens)
        {
            var newTokens = await mediator.Send(new RefreshTokenQuery(tokens));
            Console.WriteLine($"!!!\n{newTokens.accessToken}\n!!!");
            Console.WriteLine($"!!!\n{newTokens.refreshToken}\n!!!");
            if (newTokens.accessToken != null)
            {
                return Ok(newTokens);
            }
            else
            {
                return BadRequest("Tokens are expired!");
            }
        }
        [HttpPost("sign/")]
        public async Task<ActionResult<SignUpResponseDTO>> SignUp([FromBody] SignUpDTO user)
        {
            var response = await mediator.Send(new SignUpCommand(user));
            return Ok(response);
        }

    }
}
