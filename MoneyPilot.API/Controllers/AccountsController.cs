using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.BankAccounts.Add;
using MoneyPilot.Application.BankAccounts.Search;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;

namespace MoneyPilot.API.Controllers
{
    [Route("api/Users/{userId}/Accounts")]
    [ApiController]
    public class AccountsController(IMediator mediator) : ControllerBase
    {
        // Authorize pending, custom authorization logic will be required.
        [HttpPost]
        public async Task<IActionResult> AddNewAccount(int userId, [FromBody] AddAccountCommand command)
        {
            command.UserId = userId;
            var result = await mediator.SendAsync<AddAccountCommand, CustomResponse<AddAccountResult>>(command);
            return result.GetResponse();
        }

        [HttpGet("search")] // temporarily keeping GET, will introduce other filters and pagination in the future and change it to POST.
        public async Task<IActionResult> SearchAccounts(int userId)
        {
            var command = new SearchAccountsCommand(userId);
            var result = await mediator.SendAsync<SearchAccountsCommand, CustomResponse<SearchAccountsResult>>(command);
            return result.GetResponse();
        }
    }
}
