using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyPilot.Application.Investments.Add
{
    public record AddInvestmentCommand(decimal Amount, string Description, int? AutoDebitDay, int CategoryId, int? AccountId)
    {
        [JsonIgnore]
        public Guid UserOId { get; set; }
    }
}
