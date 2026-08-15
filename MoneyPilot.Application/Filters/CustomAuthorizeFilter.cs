using Microsoft.AspNetCore.Mvc.Filters;
using MoneyPilot.Application.Services;
using System.Net;

namespace MoneyPilot.Application.Filters
{
    public class CustomAuthorizeFilter(IAuthenticationService authenticationService) : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var authorizationHeader = request.Headers["Authorization"].ToString();
            if(string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
                {
                    ErrorMessage = "Authorization header is missing or invalid.",
                    StatusCode = HttpStatusCode.BadRequest,
                });
                return;
            }

            var (errorCode, errorMessage, isTokenValid) = authenticationService.ValidateToken(authorizationHeader["Bearer ".Length..].Trim());
            if(!isTokenValid)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(new
                {
                    ErrorMessage = errorMessage,
                    ErrorCode = errorCode,
                    StatusCode = HttpStatusCode.Unauthorized,
                });

                return;
            }

            return;
        }
    }
}
