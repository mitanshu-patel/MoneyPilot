using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Common.Helpers;
using MoneyPilot.Domain.Entities;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.Investments.Add
{
    public class AddInvestmentHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<AddInvestmentHandler> logger) : RequestHandlerBase<AddInvestmentCommand, CustomResponse<AddInvestmentResult>>
    {
        protected override async Task<CustomResponse<AddInvestmentResult>> ExecuteCommandAsync(AddInvestmentCommand command)
        {
            logger.LogInformation("Executing AddInvestmentCommand for user {UserOId} with amount {Amount}.", command.UserOId, command.Amount);
            try
            {
                var userDetail = await moneyPilotRepo.GetUsers()
                                .Where(v=>v.UserOId == command.UserOId)
                                .Select(v => new { v.Id })
                                .FirstOrDefaultAsync();
                if (userDetail == null)
                {
                    logger.LogWarning("User with OId {UserOId} not found.", command.UserOId);
                    return CustomHttpResult.NotFound<AddInvestmentResult>($"User not found.");
                }

                var categoryValidationResult = await TransactionHelper.ValidateCategoryAsync<AddInvestmentResult, AddInvestmentHandler>(command.CategoryId, command.AutoDebitDay, moneyPilotRepo, logger);

                if (categoryValidationResult != null)
                {
                    return categoryValidationResult;
                }

                var investment = new Investment
                {
                    CategoryId = command.CategoryId,
                    Transaction = new Transaction
                    {
                        AccountId = command.AccountId,
                        Amount = command.Amount,
                        AutoDebitDay = command.AutoDebitDay,
                        CreatedAt = DateTime.UtcNow,
                        Description = command.Description,
                        UserId = userDetail.Id
                    }
                };

                await moneyPilotRepo.SaveInvestmentAsync(investment);

                return CustomHttpResult.Ok<AddInvestmentResult>(new(investment.Id));
            }
           catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding investment.");
                throw;
            }
        }

        protected override CustomResponse<AddInvestmentResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<AddInvestmentResult>();
        }

        protected override IValidator<AddInvestmentCommand> GetValidator()
        {
            var validator = new InlineValidator<AddInvestmentCommand>
            {
                v => v.RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0."),
                v => v.RuleFor(x => x.Description).NotEmpty().MaximumLength(200).WithMessage("Description is required and must not exceed 200 characters."),
                v => v.RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0."),
            };
            return validator;
        }
    }
}
