using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyPilot.Application.BankAccounts.Update
{
    public record UpdateAccountCommand(string HolderName, long AccountNumber, decimal Balance)
    {
        [JsonIgnore]
        public Guid UserOId { get; set; }

        [JsonIgnore]
        public int AccountId { get; set; }
    }
}
