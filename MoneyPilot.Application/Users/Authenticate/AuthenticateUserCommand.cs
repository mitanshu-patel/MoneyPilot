using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.Authenticate
{
    public record AuthenticateUserCommand(string Email, string Password);
}
