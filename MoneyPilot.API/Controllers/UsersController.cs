using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.Common.DTOs;
using MoneyPilot.Application.Users.Add;
using MoneyPilot.Application.Users.Authenticate;
using MoneyPilot.Application.Users.RefreshToken;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;

namespace MoneyPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] AddUserCommand command)
        {
           
            var result = await mediator.SendAsync<AddUserCommand, CustomResponse<AddUserResult>>(command);
            return result.GetResponse();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserCommand command)
        {
            var result = await mediator.SendAsync<AuthenticateUserCommand, CustomResponse<RefreshTokenResponse>>(command);
            return result.GetResponse();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await mediator.SendAsync<RefreshTokenCommand, CustomResponse<RefreshTokenResponse>>(command);
            return result.GetResponse();
        }
    }
}
