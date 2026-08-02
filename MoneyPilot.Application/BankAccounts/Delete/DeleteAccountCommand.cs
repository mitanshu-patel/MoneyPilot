using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.BankAccounts.Delete
{
    public record DeleteAccountCommand(int UserId, int Id);
}
