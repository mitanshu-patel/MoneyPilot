using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Add
{
    public record AddAccountCommand(int UserId, string HolderName, long AccountNumber, decimal Balance);
}
