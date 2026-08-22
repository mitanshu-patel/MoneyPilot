using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Common.DTOs;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.Investments.Get
{
    public class GetInvestmentDetailsHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<GetInvestmentDetailsHandler> logger) : RequestHandlerBase<GetInvestmentDetailsQuery, CustomResponse<GetInvestmentDetailsResult>>
    {
        protected override async Task<CustomResponse<GetInvestmentDetailsResult>> ExecuteCommandAsync(GetInvestmentDetailsQuery command)
        {
            logger.LogInformation("Executing GetInvestmentDetailsHandler for UserOId: {UserOId}, Id: {Id}", command.UserOId, command.Id);
            try
            {
                var investmentDetails = await moneyPilotRepo.GetInvestments()
                    .Where(i => i.Transaction.User.UserOId == command.UserOId && i.Id == command.Id)
                    .Select(i => new TransactionDetail
                    {
                        AccountId = i.Transaction.AccountId,
                        Amount = i.Transaction.Amount,
                        AutoDebitDay = i.Transaction.AutoDebitDay,
                        Description = i.Transaction.Description,
                        CategoryId = i.CategoryId,
                    })
                    .FirstOrDefaultAsync();

                if(investmentDetails == null)
                {
                    logger.LogWarning("Investment details not found for UserOId: {UserOId}, Id: {Id}", command.UserOId, command.Id);
                    return CustomHttpResult.NotFound<GetInvestmentDetailsResult>("Investment details not found.");
                }

                logger.LogInformation("Successfully retrieved investment details for UserOId: {UserOId}, Id: {Id}", command.UserOId, command.Id);

                return CustomHttpResult.Ok<GetInvestmentDetailsResult>(new(investmentDetails));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing GetInvestmentDetailsHandler.");
                throw;
            }
        }

        protected override CustomResponse<GetInvestmentDetailsResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<GetInvestmentDetailsResult>();
        }

        protected override IValidator<GetInvestmentDetailsQuery> GetValidator()
        {
            var validator = new InlineValidator<GetInvestmentDetailsQuery>();
            validator.RuleFor(x => x.UserOId).NotEmpty().WithMessage("UserOId is required.");
            validator.RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            return validator;
        }
    }
}
