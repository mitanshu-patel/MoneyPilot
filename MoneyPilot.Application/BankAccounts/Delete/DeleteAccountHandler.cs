using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Delete
{
    public class DeleteAccountHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<DeleteAccountHandler> logger) : RequestHandlerBase<DeleteAccountCommand, CustomResponse<DeleteAccountResult>>
    {
        protected override async Task<CustomResponse<DeleteAccountResult>> ExecuteCommandAsync(DeleteAccountCommand command)
        {
            logger.LogInformation("Deleting account with Id {Id} for UserId {UserId}.", command.Id, command.UserOId);
            try
            {
                var account = await moneyPilotRepo.GetBankAccounts().Where(v => v.Id == command.Id && v.User.UserOId == command.UserOId).FirstOrDefaultAsync();
                if(account == null)
                {
                    logger.LogWarning("Account with Id {Id} for UserId {UserId} not found.", command.Id, command.UserOId);
                    return CustomHttpResult.NotFound<DeleteAccountResult>($"Account not found.");
                }

                await moneyPilotRepo.DeleteAccountAsync(account);
                logger.LogInformation("Account with Id {Id} for UserId {UserId} deleted successfully.", command.Id, command.UserOId);
                return CustomHttpResult.Ok(new DeleteAccountResult());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the account with Id {Id} for UserId {UserId}.", command.Id, command.UserOId);
                throw;
            }
        }

        protected override CustomResponse<DeleteAccountResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<DeleteAccountResult>();
        }

        protected override IValidator<DeleteAccountCommand> GetValidator()
        {
            var validator = new InlineValidator<DeleteAccountCommand>();
            validator.RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            return validator;
        }
    }
}
