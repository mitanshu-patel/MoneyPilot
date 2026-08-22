using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Common.DTOs
{
    public record TransactionDetail
    {
        public int? AccountId { get; init; }

        public int CategoryId { get; init; }

        public decimal Amount { get; init; }

        public int? AutoDebitDay { get; init; }

        public string Description { get; init; } = string.Empty;
    }
}
