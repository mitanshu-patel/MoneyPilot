using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Delete
{
    public class DeleteInvestmentHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<DeleteInvestmentHandler> logger) : RequestHandlerBase<DeleteInvestmentCommand, CustomResponse<DeleteInvestmentResult>>
    {
        protected override async Task<CustomResponse<DeleteInvestmentResult>> ExecuteCommandAsync(DeleteInvestmentCommand command)
        {
            logger.LogInformation("Deleting investment with Id: {InvestmentId}", command.Id);
            try
            {
                var investment = await moneyPilotRepo.GetInvestments()
                                .FirstOrDefaultAsync(v=>v.Id == command.Id && v.Transaction.User.UserOId == command.UserOId);

                if(investment == null)
                {
                    logger.LogWarning("Investment with Id: {InvestmentId} not found for UserOId: {UserOId}", command.Id, command.UserOId);
                    return CustomHttpResult.NotFound<DeleteInvestmentResult>($"Investment not found.");
                }

                await moneyPilotRepo.DeleteInvestmentAsync(investment);
                logger.LogInformation("Investment with Id: {InvestmentId} deleted successfully.", command.Id);
                return CustomHttpResult.Ok(new DeleteInvestmentResult());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting investment with Id: {InvestmentId}", command.Id);
                throw;
            }
        }

        protected override CustomResponse<DeleteInvestmentResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<DeleteInvestmentResult>();
        }

        protected override IValidator<DeleteInvestmentCommand> GetValidator()
        {
            var validator = new InlineValidator<DeleteInvestmentCommand>
            {
                v => v.RuleFor(x => x.Id).GreaterThan(0).WithMessage("InvestmentId must be greater than 0.")
            };
            return validator;
        }
    }
}
