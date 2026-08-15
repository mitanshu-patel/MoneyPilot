using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.BankAccounts.Get.DTOs;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.BankAccounts.Get
{
    public class GetAccountDetailsHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<GetAccountDetailsHandler> logger) : RequestHandlerBase<GetAccountDetailsQuery, CustomResponse<GetAccountDetailsResult>>
    {
        protected async override Task<CustomResponse<GetAccountDetailsResult>> ExecuteCommandAsync(GetAccountDetailsQuery command)
        {
            logger.LogInformation("Executing GetAccountDetailsHandler for Id: {Id}", command.Id);
            try
            {
                var accountDetail = await moneyPilotRepo.GetBankAccounts()
                                    .Where(v => v.Id == command.Id && v.User.UserOId == command.UserOId)
                                    .Select(v => new AccountDetailsDto
                                    {
                                        Id = v.Id,
                                        HolderName = v.HolderName,
                                        Balance = v.Balance,
                                        AccountNumber = v.AccountNumber
                                    }).FirstOrDefaultAsync();

                if (accountDetail == null)
                {
                    logger.LogWarning("No account details found for Id: {Id} and UserId: {UserId}", command.Id, command.UserOId);
                    return CustomHttpResult.NotFound<GetAccountDetailsResult>("Account details not found.");

                }
                logger.LogInformation("Successfully retrieved account details for Id: {Id} and UserId: {UserId}", command.Id, command.UserOId);
                return CustomHttpResult.Ok<GetAccountDetailsResult>(new(accountDetail));
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "An error occurred while getting account details.");
                throw;
            }
        }

        protected override CustomResponse<GetAccountDetailsResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<GetAccountDetailsResult>();
        }

        protected override IValidator<GetAccountDetailsQuery> GetValidator()
        {
            var validator = new InlineValidator<GetAccountDetailsQuery>();
            validator.RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            return validator;
        }
    }
}
