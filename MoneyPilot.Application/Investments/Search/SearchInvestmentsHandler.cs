using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Investments.Search.DTOs;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Search
{
    public class SearchInvestmentsHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<SearchInvestmentsHandler> logger) : RequestHandlerBase<SearchInvestmentsCommand, CustomResponse<SearchInvestmentsResult>>
    {
        protected override async Task<CustomResponse<SearchInvestmentsResult>> ExecuteCommandAsync(SearchInvestmentsCommand command)
        {
            logger.LogInformation("Executing SearchInvestmentsHandler for UserOId: {UserOId}", command.UserOId);
            try
            {
                var userExists = await moneyPilotRepo.GetUsers().AnyAsync(t => t.UserOId == command.UserOId);
                if (!userExists)
                {
                    logger.LogWarning("User with UserOId: {UserOId} not found.", command.UserOId);
                    return CustomHttpResult.NotFound<SearchInvestmentsResult>($"User not found.");
                }

                var investments = await moneyPilotRepo.GetInvestments()
                    .Where(t => t.Transaction.User.UserOId == command.UserOId)
                    .Select(v => new InvestmentsDto
                    {
                        Category = v.Category.Category,
                        Amount = v.Transaction.Amount,
                        Details = v.Transaction.Description,
                        Account = v.Transaction.Account,
                        AutoDebitDay = v.Transaction.AutoDebitDay,
                    })
                    .ToListAsync();

                logger.LogInformation("Found {Count} investments for UserOId: {UserOId}", investments.Count, command.UserOId);
                return CustomHttpResult.Ok<SearchInvestmentsResult>(new(investments));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing SearchInvestmentsHandler for UserOId: {UserOId}", command.UserOId);
                throw;
            }
        }

        protected override CustomResponse<SearchInvestmentsResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<SearchInvestmentsResult>();
        }

        protected override IValidator<SearchInvestmentsCommand> GetValidator()
        {
           return new InlineValidator<SearchInvestmentsCommand>
            {
                v => v.RuleFor(x => x.UserOId).NotEqual(Guid.Empty).WithMessage("UserOId is required.")
            };
        }
    }
}
