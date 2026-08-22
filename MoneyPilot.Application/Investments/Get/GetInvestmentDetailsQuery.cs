using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Get
{
    public record GetInvestmentDetailsQuery(Guid UserOId, int Id);
}
