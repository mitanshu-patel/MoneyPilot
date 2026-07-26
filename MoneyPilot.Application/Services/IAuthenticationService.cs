using MoneyPilot.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyPilot.Application.Services
{
    public interface IAuthenticationService
    {
        Task<RefreshTokenResponse> GenerateNewTokenAsync(Guid userOId, string email);

        Task<(RefreshTokenResponse? RefreshToken, string ErrorMessage)> GenerateJwtAndRefreshTokenAsync(string accessToken, string refreshToken);
    }
}
