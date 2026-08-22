using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.Filters;
using MoneyPilot.Application.Investments.Add;
using MoneyPilot.Application.Investments.Delete;
using MoneyPilot.Application.Investments.Get;
using MoneyPilot.Application.Investments.Search;
using MoneyPilot.Application.Investments.Update;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;
using MoneyPilot.Shared.Services;

namespace MoneyPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomAuthorizeFilter))]
    public class InvestmentsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddNewInvestment([FromBody] AddInvestmentCommand command)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            command.UserOId = Guid.Parse(userOid);
            var result = await mediator.SendAsync<AddInvestmentCommand, CustomResponse<AddInvestmentResult>>(command);
            return result.GetResponse();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvestment(int id, [FromBody] UpdateInvestmentCommand command)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            command.UserOId = Guid.Parse(userOid);
            command.Id = id;
            var result = await mediator.SendAsync<UpdateInvestmentCommand, CustomResponse<UpdateInvestmentResult>>(command);
            return result.GetResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new DeleteInvestmentCommand(Guid.Parse(userOid), id);
            var result = await mediator.SendAsync<DeleteInvestmentCommand, CustomResponse<DeleteInvestmentResult>>(command);
            return result.GetResponse();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvestmentDetails(int id)
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new GetInvestmentDetailsQuery(Guid.Parse(userOid), id);
            var result = await mediator.SendAsync<GetInvestmentDetailsQuery, CustomResponse<GetInvestmentDetailsResult>>(command);
            return result.GetResponse();
        }

        [HttpGet("search")] // temporarily keeping GET, will introduce other filters and pagination in the future and change it to POST.
        public async Task<IActionResult> SearchInvestments()
        {
            var userOid = Request.GetCurrentUserIdFromAuthorizationHeader();
            if (string.IsNullOrEmpty(userOid))
            {
                return Unauthorized();
            }

            var command = new SearchInvestmentsCommand(Guid.Parse(userOid));
            var result = await mediator.SendAsync<SearchInvestmentsCommand, CustomResponse<SearchInvestmentsResult>>(command);
            return result.GetResponse();
        }
    }
}
