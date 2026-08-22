using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.RefreshToken
{
    public record RefreshTokenCommand(string Token, string RefreshToken);
}
