using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Delete
{
    public record DeleteInvestmentCommand(Guid UserOId, int Id);
}
