using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Domain.Entities;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Add
{
    public class AddAccountHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<AddAccountHandler> logger) : RequestHandlerBase<AddAccountCommand, CustomResponse<AddAccountResult>>
    {
        protected override async Task<CustomResponse<AddAccountResult>> ExecuteCommandAsync(AddAccountCommand command)
        {
            logger.LogInformation("Executing AddAccountCommand for UserId: {UserId}", command.UserOId);
            try
            {
                var userDetail = await moneyPilotRepo.GetUsers()
                                .Where(t => t.UserOId == command.UserOId)
                                .Select(t=>new {t.Id})
                                .FirstOrDefaultAsync();
                if (userDetail == null)
                {
                    logger.LogWarning("User not found for UserId: {UserId}", command.UserOId);
                    return CustomHttpResult.NotFound<AddAccountResult>("User not found.");
                }

                var accountExists = await moneyPilotRepo.GetBankAccounts().AnyAsync(t => t.AccountNumber == command.AccountNumber);
                if (accountExists)
                {
                    logger.LogWarning("Account already exists for AccountNumber: {AccountNumber}", command.AccountNumber);
                    throw new InvalidOperationException("Account already exists.");
                }

                var account = new BankAccount
                {
                    UserId = userDetail.Id,
                    HolderName = command.HolderName,
                    AccountNumber = command.AccountNumber,
                    Balance = command.Balance
                };

                await moneyPilotRepo.SaveBankAccountAsync(account);
                logger.LogInformation("Account added successfully for UserId: {UserId}, AccountNumber: {AccountNumber}", command.UserOId, command.AccountNumber);
                return CustomHttpResult.Ok<AddAccountResult>(new(account.Id));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding account for UserId: {UserId}", command.UserOId);
                throw;
            }
        }

        protected override CustomResponse<AddAccountResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<AddAccountResult>();
        }

        protected override IValidator<AddAccountCommand> GetValidator()
        {
            var validator = new InlineValidator<AddAccountCommand>();
            validator.RuleFor(x => x.HolderName).NotEmpty().WithMessage("HolderName is required.");
            validator.RuleFor(x => x.AccountNumber).GreaterThan(0).WithMessage("AccountNumber must be greater than 0.");
            return validator;
        }
    }
}
