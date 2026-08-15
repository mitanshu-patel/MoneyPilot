using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Get
{
    public record GetAccountDetailsQuery(Guid UserOId, int Id);
}
