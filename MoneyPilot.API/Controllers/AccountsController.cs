using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.BankAccounts.Add;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;

namespace MoneyPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(IMediator mediator) : ControllerBase
    {
        // Authorize pending, custom authorization logic will be required.
        [HttpPost]
        public async Task<IActionResult> AddNewAccount([FromBody] AddAccountCommand command)
        {
            var result = await mediator.SendAsync<AddAccountCommand, CustomResponse<AddAccountResult>>(command);
            return result.GetResponse();
        }
    }
}
