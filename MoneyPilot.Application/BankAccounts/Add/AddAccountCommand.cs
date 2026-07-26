using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyPilot.Application.BankAccounts.Add
{
    public record AddAccountCommand(string HolderName, long AccountNumber, decimal Balance)
    {
        [JsonIgnore]
        public  int  UserId { get; set; }
    }
}
