using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Search.DTOs
{
    public record SearchAccountsDto
    {
        public int AccountId { get; init; }

        public string HolderName { get; init; }

        public decimal Balance { get; set; }
    }
}
