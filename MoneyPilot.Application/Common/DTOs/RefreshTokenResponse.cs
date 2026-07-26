using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyPilot.Application.Common.DTOs
{
    public record RefreshTokenResponse(string AccessToken, string RefreshToken);
}
