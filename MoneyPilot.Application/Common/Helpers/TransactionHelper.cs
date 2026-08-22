using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyPilot.Application.Investments.Add;
using MoneyPilot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Common.Helpers
{
    public static class TransactionHelper
    {
        public static async Task<CustomResponse<TResult>?> ValidateCategoryAsync<TResult, TLogger>(int categoryId, int? autoDebitDay, IMoneyPilotRepo moneyPilotRepo, ILogger<TLogger> logger)
        {
            var categoryDetail = await moneyPilotRepo.GetInvestmentCategories()
                                    .Where(t => t.Id == categoryId)
                                    .Select(v => new { v.HasAutoPayment }).FirstOrDefaultAsync();

            if (categoryDetail == null)
            {
                logger.LogWarning("Investment category with Id {CategoryId} not found.", categoryId);
                return CustomHttpResult.NotFound<TResult>($"Investment category not found.");
            }

            if (categoryDetail.HasAutoPayment && autoDebitDay == null)
            {
                logger.LogWarning("Auto-debit day is required for investment category with Id {CategoryId}.", categoryId);
                return CustomHttpResult.BadRequest<TResult>("Auto-debit day is required for this investment category.");
            }

            return null;
        }
    }
}
