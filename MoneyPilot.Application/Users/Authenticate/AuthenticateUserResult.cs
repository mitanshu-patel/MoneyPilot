using MoneyPilot.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.Authenticate
{
    public record AuthenticateUserResult(int Id, RefreshTokenResponse TokenDetails);
}
