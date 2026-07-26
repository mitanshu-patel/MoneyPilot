using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyPilot.Application.Common.Enums;
using MoneyPilot.Application.Lookup;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;

namespace MoneyPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{lookupType}")]
        public async Task<IActionResult> GetLookups([FromRoute] LookupTypeEnums lookupType)
        {
            var command = new LookupQuery(lookupType);
            var result = await mediator.SendAsync<LookupQuery, CustomResponse<LookupResult>>(command);
            return result.GetResponse();
        }
    }
}
