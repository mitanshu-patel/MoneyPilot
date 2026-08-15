using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace MoneyPilot.Shared.Common
{
    public static class Extensions
    {
        private static Dictionary<string, List<string>> GetValidationErrors(this List<ValidationFailure> errors)
        {
            return errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToList()
                );
        }

        public static CustomResponse<TResult> GetValidationResult<TResult>(this ValidationResult validationResult)
        {
            return CustomHttpResult.BadRequest<TResult>("One or more validation errors", validationResult.Errors.GetValidationErrors());
        }

        public static T? GetConfigurationValue<T>(this IConfiguration configuration, string valueKey)
        {
            var valueSection = configuration.GetSection("Values");
            if (valueSection == null)
            {
                return configuration.GetValue<T>(valueKey);
            }

            return valueSection.GetValue<T>(valueKey);
        }

        public static string GetCurrentUserIdFromAuthorizationHeader(this HttpRequest request)
        {
            if(!request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDetails = tokenHandler.ReadJwtToken(authHeader.ToString().Replace("Bearer ", ""));
            var userId = tokenDetails.Claims.Where(v=>v.Type == "userId").Select(v => v.Value).FirstOrDefault();
            return userId ?? string.Empty;
        }
    }
}
