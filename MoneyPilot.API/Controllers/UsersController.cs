using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.Users.Add;
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
    }
}
