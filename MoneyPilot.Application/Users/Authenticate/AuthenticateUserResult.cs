using MoneyPilot.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Users.Authenticate
{
    public record AuthenticateUserResult(RefreshTokenResponse TokenDetails);
}
