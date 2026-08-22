using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyPilot.Application.Investments.Update
{
    public record UpdateInvestmentCommand(decimal Amount, string Description, int? AutoDebitDay, int CategoryId, int? AccountId)
    {
        [JsonIgnore]
        public Guid UserOId { get; set; }

        [JsonIgnore]
        public int Id { get; set; } = 0;
    }
}
