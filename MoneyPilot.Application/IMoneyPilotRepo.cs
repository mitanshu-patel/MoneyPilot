using MoneyPilot.Domain.Entities;

namespace MoneyPilot.Application
{
    public interface IMoneyPilotRepo
    {
        public IQueryable<User> GetUsers();
        public Task<int> AddNewUserAsync(User user);

        public IQueryable<RefreshToken> GetRefreshTokens();

        public Task SaveRefreshTokenAsync(RefreshToken refreshToken);

        public IQueryable<BankAccount> GetBankAccounts();

        public Task SaveBankAccountAsync(BankAccount bankAccount);

        public IQueryable<InvestmentCategory> GetInvestmentCategories();

        public IQueryable<ExpenseCategory> GetExpenseCategories();
    }
}
