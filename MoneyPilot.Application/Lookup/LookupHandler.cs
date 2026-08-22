using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Common.Enums;
using MoneyPilot.Application.Lookup.DTOs;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.Lookup
{
    public class LookupHandler(IMoneyPilotRepo moneyPilotRepo, ILogger<LookupHandler> logger) : RequestHandlerBase<LookupQuery, CustomResponse<LookupResult>>
    {
        protected override async Task<CustomResponse<LookupResult>> ExecuteCommandAsync(LookupQuery command)
        {
            logger.LogDebug("Executing LookupHandler with command: {@Command}", command);
            try
            {
                var lookups = command.LookupType switch
                {
                    LookupTypeEnums.InvestmentCategory => moneyPilotRepo
                    .GetInvestmentCategories()
                    .Select(x => new LookupDto(x.Id, x.Category)),
                    LookupTypeEnums.ExpenseCategory => moneyPilotRepo
                    .GetExpenseCategories()
                    .Select(x => new LookupDto(x.Id, x.Category)),
                    LookupTypeEnums.Accounts => moneyPilotRepo
                   .GetBankAccounts()
                   .Select(x => new LookupDto(x.Id, $"{x.HolderName} ({x.AccountNumber})")),
                    LookupTypeEnums.None => throw new NotImplementedException(),
                    _ => throw new ArgumentOutOfRangeException(nameof(command.LookupType), "Invalid lookup type.")
                };

                var lookupList = await lookups.ToListAsync();
                logger.LogDebug("LookupHandler executed successfully. Retrieved {Count} items.", lookupList.Count);
                return CustomHttpResult.Ok<LookupResult>(new(lookupList));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing LookupHandler.");
                throw;
            }
        }

        protected override CustomResponse<LookupResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<LookupResult>();
        }

        protected override IValidator<LookupQuery> GetValidator()
        {
            var validator = new InlineValidator<LookupQuery>();
            validator.RuleFor(x => x.LookupType)
                .IsInEnum()
                .WithMessage("Invalid lookup type.");
            return validator;
        }
    }
}
