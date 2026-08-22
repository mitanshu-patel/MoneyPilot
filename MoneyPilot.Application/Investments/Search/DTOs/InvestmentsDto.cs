using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyPilot.Application.Investments.Search.DTOs
{
    public record InvestmentsDto
    {
        public int Id { get; init; }

        [JsonIgnore]
        public BankAccount? Account { get; init; }

        public long AccountNumber { get => this.Account?.AccountNumber ?? 0; }

        public string HolderName { get => this.Account?.HolderName ?? string.Empty; }

        public decimal Amount { get; init; }

        public string Category { get; init; } = string.Empty;

        public string Details { get; init; } = string.Empty;

        public int? AutoDebitDay { get; init; }
    }
}
