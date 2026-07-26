using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Services;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.Authenticate
{
    public class AuthenticateUserHandler(IMoneyPilotRepo moneyPilotRepo, IAuthenticationService authenticationService, ILogger<AuthenticateUserHandler> logger) : RequestHandlerBase<AuthenticateUserCommand, CustomResponse<AuthenticateUserResult>>
    {
        protected async override Task<CustomResponse<AuthenticateUserResult>> ExecuteCommandAsync(AuthenticateUserCommand command)
        {
            logger.LogDebug("Executing AuthenticateUserHandler for Email: {Email}", command.Email);
            try
            {
                var hashPassword = command.Password.ComputeSHA256Hash();
                var user = await moneyPilotRepo.GetUsers().Where(v => v.Email.Equals(command.Email))
                            .Select(v => new { v.Id, v.Email, v.UserOId, v.Password })
                            .FirstOrDefaultAsync();
                if (user == null)
                {
                    logger.LogWarning("User not found for Email: {Email}", command.Email);
                    return CustomHttpResult.NotFound<AuthenticateUserResult>("User not found.");
                }

                if (user.Password != hashPassword)
                {
                    logger.LogWarning("Invalid password for Email: {Email}", command.Email);
                    return CustomHttpResult.UnAuthorized<AuthenticateUserResult>("Invalid Email or Password.");
                }

                var tokenDetail = await authenticationService.GenerateNewTokenAsync(user.UserOId, user.Email);
                logger.LogInformation("User authenticated successfully for Email: {Email}", command.Email);
                return CustomHttpResult.Ok<AuthenticateUserResult>(new(user.Id, tokenDetail));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while authenticating user for Email: {Email}", command.Email);
                throw;
            }
        }

        protected override CustomResponse<AuthenticateUserResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<AuthenticateUserResult>();
        }

        protected override IValidator<AuthenticateUserCommand> GetValidator()
        {
           var validator = new InlineValidator<AuthenticateUserCommand>();
            validator.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            validator.RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
                
            return validator;
        }
    }
}
