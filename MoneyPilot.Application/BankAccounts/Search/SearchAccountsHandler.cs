using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.BankAccounts.Search.DTOs;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Search
{
    public class SearchAccountsHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<SearchAccountsHandler> logger) : RequestHandlerBase<SearchAccountsCommand, CustomResponse<SearchAccountsResult>>
    {
        protected async override Task<CustomResponse<SearchAccountsResult>> ExecuteCommandAsync(SearchAccountsCommand command)
        {
            logger.LogDebug("Executing SearchAccountsHandler with command: {@Command}", command);
            try
            {
                var userExists = await moneyPilotRepo.GetUsers().AnyAsync(v => v.Id == command.UserId);
                if (!userExists)
                {
                    logger.LogWarning("User with ID {UserId} not found.", command.UserId);
                    return CustomHttpResult.NotFound<SearchAccountsResult>($"User with ID {command.UserId} not found.");
                }

                var accounts = await moneyPilotRepo.GetBankAccounts().Where(v => v.UserId == command.UserId).Select(v => new SearchAccountsDto
                {
                    AccountId = v.Id,
                    HolderName = v.HolderName,
                    Balance = v.Balance,
                }).ToListAsync();

                logger.LogDebug("Retrieved {Count} accounts for user {UserId}", accounts.Count, command.UserId);
                return CustomHttpResult.Ok<SearchAccountsResult>(new(accounts));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing SearchAccountsHandler.");
                throw;
            }
        }

        protected override CustomResponse<SearchAccountsResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<SearchAccountsResult>();
        }

        protected override IValidator<SearchAccountsCommand> GetValidator()
        {
            var validator = new InlineValidator<SearchAccountsCommand>();
            validator.RuleFor(v => v.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            return validator;
        }
    }
}
