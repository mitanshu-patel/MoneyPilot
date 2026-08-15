using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.BankAccounts.Update
{
    public class UpdateAccountHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<UpdateAccountHandler> logger) : RequestHandlerBase<UpdateAccountCommand, CustomResponse<UpdateAccountResult>>
    {
        protected async override Task<CustomResponse<UpdateAccountResult>> ExecuteCommandAsync(UpdateAccountCommand command)
        {
            logger.LogInformation("Executing UpdateAccountHandler for UserId: {UserId}, AccountId: {AccountId}", command.UserOId, command.AccountId);
            try
            {
                var account = await moneyPilotRepo.GetBankAccounts()
                            .Where(v=>v.Id == command.AccountId && v.User.UserOId == command.UserOId)
                            .FirstOrDefaultAsync();

                if (account == null)
                {
                    logger.LogWarning("Bank account not found for UserId: {UserId}, AccountId: {AccountId}", command.UserOId, command.AccountId);
                    return CustomHttpResult.NotFound<UpdateAccountResult>("Bank account not found.");
                }

                account.HolderName = command.HolderName;
                account.AccountNumber = command.AccountNumber;
                account.Balance = command.Balance;

                await moneyPilotRepo.SaveBankAccountAsync(account);

                logger.LogInformation("Bank account updated successfully for UserId: {UserId}, AccountId: {AccountId}", command.UserOId, command.AccountId);
                return CustomHttpResult.Ok<UpdateAccountResult>(new());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating the account.");
                throw;
            }
        }

        protected override CustomResponse<UpdateAccountResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<UpdateAccountResult>();
        }

        protected override IValidator<UpdateAccountCommand> GetValidator()
        {
            var validator = new InlineValidator<UpdateAccountCommand>();
            validator.RuleFor(x => x.AccountId).GreaterThan(0).WithMessage("AccountId must be greater than 0.");
            validator.RuleFor(x => x.HolderName).NotEmpty().WithMessage("HolderName is required.");
            validator.RuleFor(x => x.AccountNumber).GreaterThan(0).WithMessage("AccountNumber must be greater than 0.");
            return validator;
        }
    }
}
