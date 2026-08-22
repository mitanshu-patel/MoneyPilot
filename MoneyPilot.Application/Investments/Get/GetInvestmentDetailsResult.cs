using MoneyPilot.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Get
{
    public record GetInvestmentDetailsResult(TransactionDetail InvestmentDetails);
}
