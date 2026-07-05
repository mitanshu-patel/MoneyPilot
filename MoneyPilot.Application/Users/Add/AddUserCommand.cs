using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.Add
{
    public record AddUserCommand(string Email, string Password);
}
