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
                var userDetail = await moneyPilotRepo.GetUsers()
                                .Where(v => v.UserOId == command.UserOId)
                                .Select(t => new { t.Id })
                                .FirstOrDefaultAsync();
                if (userDetail == null)
                {
                    logger.LogWarning("User with ID {UserId} not found.", command.UserOId);
                    return CustomHttpResult.NotFound<SearchAccountsResult>($"User with ID {command.UserOId} not found.");
                }

                var accounts = await moneyPilotRepo.GetBankAccounts().Where(v => v.UserId == userDetail.Id).Select(v => new SearchAccountsDto
                {
                    AccountId = v.Id,
                    HolderName = v.HolderName,
                    Balance = v.Balance,
                }).ToListAsync();

                logger.LogDebug("Retrieved {Count} accounts for user {UserId}", accounts.Count, command.UserOId);
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
            return validator;
        }
    }
}
