using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Common.DTOs;
using MoneyPilot.Application.Services;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.RefreshToken
{
    internal class RefreshTokensHandler(IAuthenticationService authenticationService, ILogger<RefreshTokensHandler> logger) : RequestHandlerBase<RefreshTokenCommand, CustomResponse<RefreshTokenResponse>>
    {
        protected override async Task<CustomResponse<RefreshTokenResponse>> ExecuteCommandAsync(RefreshTokenCommand command)
        {
            try
            {
                logger.LogInformation("RefreshTokensHandler: ExecuteCommandAsync execution started");
                var (refreshTokenResponse, errorMessage) = await authenticationService.GenerateJwtAndRefreshTokenAsync(command.Token, command.RefreshToken);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    logger.LogError("Error while generating refresh token, error:{error}", errorMessage);
                    return CustomHttpResult.UnAuthorized<RefreshTokenResponse>(errorMessage);
                }

                logger.LogInformation("RefreshTokensHandler: ExecuteCommandAsync successfully generated new refresh token");
                return CustomHttpResult.Ok(refreshTokenResponse!);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RefreshTokensHandler: exception occurred with message:{message}", ex.Message);
                throw;
            }
        }

        protected override CustomResponse<RefreshTokenResponse> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<RefreshTokenResponse>();
        }

        protected override IValidator<RefreshTokenCommand> GetValidator()
        {
            var validator = new InlineValidator<RefreshTokenCommand>();
            validator.RuleFor(v => v.Token).NotEmpty().WithMessage("Token is required.");
            validator.RuleFor(v => v.RefreshToken).NotEmpty().WithMessage("RefreshToken is required.");
            return validator;
        }
    }
}
