using MoneyPilot.Application.Investments.Search.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Search
{
    public record SearchInvestmentsResult(List<InvestmentsDto> Investments);
}
