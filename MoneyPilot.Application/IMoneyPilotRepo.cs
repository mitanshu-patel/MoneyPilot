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

        public Task DeleteAccountAsync(BankAccount bankAccount);

        public IQueryable<Investment> GetInvestments();

        public Task SaveInvestmentAsync(Investment investment);

        public Task DeleteInvestmentAsync(Investment investment);

        public Task DeleteExpenseAsync(Expense expense);
    }
}
