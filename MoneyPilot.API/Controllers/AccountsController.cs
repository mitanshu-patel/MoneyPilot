using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.BankAccounts.Add;
using MoneyPilot.Application.BankAccounts.Delete;
using MoneyPilot.Application.BankAccounts.Get;
using MoneyPilot.Application.BankAccounts.Search;
using MoneyPilot.Application.BankAccounts.Update;
using MoneyPilot.Application.Filters;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;

namespace MoneyPilot.API.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    [TypeFilter(typeof(CustomAuthorizeFilter))]
    public class AccountsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddNewAccount([FromBody] AddAccountCommand command)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            command.UserOId = Guid.Parse(userOid);
            var result = await mediator.SendAsync<AddAccountCommand, CustomResponse<AddAccountResult>>(command);
            return result.GetResponse();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountCommand command)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            command.UserOId = Guid.Parse(userOid);
            command.AccountId = id;
            var result = await mediator.SendAsync<UpdateAccountCommand, CustomResponse<UpdateAccountResult>>(command);
            return result.GetResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new DeleteAccountCommand(Guid.Parse(userOid), id);
            var result = await mediator.SendAsync<DeleteAccountCommand, CustomResponse<DeleteAccountResult>>(command);
            return result.GetResponse();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountDetails(int id)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new GetAccountDetailsQuery(Guid.Parse(userOid), id);
            var result = await mediator.SendAsync<GetAccountDetailsQuery, CustomResponse<GetAccountDetailsResult>>(command);
            return result.GetResponse();
        }

        [HttpGet("search")] // temporarily keeping GET, will introduce other filters and pagination in the future and change it to POST.
        public async Task<IActionResult> SearchAccounts()
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new SearchAccountsCommand(Guid.Parse(userOid));
            var result = await mediator.SendAsync<SearchAccountsCommand, CustomResponse<SearchAccountsResult>>(command);
            return result.GetResponse();
        }
    }
}
