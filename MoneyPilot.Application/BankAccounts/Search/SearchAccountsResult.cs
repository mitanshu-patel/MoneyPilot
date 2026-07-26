using MoneyPilot.Application.BankAccounts.Search.DTOs;

namespace MoneyPilot.Application.BankAccounts.Search
{
    public record SearchAccountsResult(List<SearchAccountsDto> Accounts);
}
