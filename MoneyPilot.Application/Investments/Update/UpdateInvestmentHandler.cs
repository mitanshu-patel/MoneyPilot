using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Update
{
    public class UpdateInvestmentHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<UpdateInvestmentHandler> logger) : RequestHandlerBase<UpdateInvestmentCommand, CustomResponse<UpdateInvestmentResult>>
    {
        protected override async Task<CustomResponse<UpdateInvestmentResult>> ExecuteCommandAsync(UpdateInvestmentCommand command)
        {
            logger.LogInformation("Executing UpdateInvestmentHandler for InvestmentId: {InvestmentId}", command.Id);
            try
            {
                var investment = await moneyPilotRepo.GetInvestments().FirstOrDefaultAsync(v=>v.Id == command.Id && v.Transaction.User.UserOId == command.UserOId);
                if (investment == null)
                {
                    logger.LogWarning("Investment not found for InvestmentId: {InvestmentId}", command.Id);
                    throw new InvalidOperationException("Investment not found.");
                }

                investment.Transaction.Amount = command.Amount;
                investment.CategoryId = command.CategoryId;
                investment.Transaction.Description = command.Description;
                investment.Transaction.AutoDebitDay = command.AutoDebitDay;
                investment.Transaction.AccountId = command.AccountId;
                investment.Transaction.ModifiedAt = DateTime.UtcNow;

                await moneyPilotRepo.SaveInvestmentAsync(investment);

                logger.LogInformation("Investment updated successfully for InvestmentId: {InvestmentId}", command.Id);

                return CustomHttpResult.Ok(new UpdateInvestmentResult());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating the investment.");
                throw;
            }
        }

        protected override CustomResponse<UpdateInvestmentResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<UpdateInvestmentResult>();
        }

        protected override IValidator<UpdateInvestmentCommand> GetValidator()
        {
            var validator = new InlineValidator<UpdateInvestmentCommand>
            {
                v => v.RuleFor(x => x.Id).GreaterThan(0).WithMessage("InvestmentId must be greater than 0."),
                v => v.RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0."),
                v => v.RuleFor(x => x.Description).NotEmpty().MaximumLength(200).WithMessage("Description is required and must not exceed 200 characters."),
                v => v.RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0."),
                v => v.RuleFor(x => x.UserOId).NotEmpty().WithMessage("UserOId is required.")
            };
            return validator;
        }
    }
}
