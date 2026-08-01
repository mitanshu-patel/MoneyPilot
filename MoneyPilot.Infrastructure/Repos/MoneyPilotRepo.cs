using MoneyPilot.Application;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.Repos
{
    public class MoneyPilotRepo(MoneyPilotContext moneyPilotContext) : IMoneyPilotRepo
    {
        public async Task<int> AddNewUserAsync(User user)
        {
           moneyPilotContext.Users.Add(user);
           await moneyPilotContext.SaveChangesAsync();
           return user.Id;
        }

        public IQueryable<BankAccount> GetBankAccounts()
        {
            return moneyPilotContext.BankAccounts.AsQueryable();
        }

        public async Task SaveBankAccountAsync(BankAccount bankAccount)
        {
            if (bankAccount.Id == 0)
            {
                moneyPilotContext.BankAccounts.Add(bankAccount);
            }
            await moneyPilotContext.SaveChangesAsync();
        }

        public IQueryable<RefreshToken> GetRefreshTokens()
        {
            return moneyPilotContext.RefreshTokens.AsQueryable();
        }

        public IQueryable<User> GetUsers()
        {
            return moneyPilotContext.Users.AsQueryable();
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            moneyPilotContext.RefreshTokens.Add(refreshToken);
            await moneyPilotContext.SaveChangesAsync();
        }

        public IQueryable<InvestmentCategory> GetInvestmentCategories()
        {
           return moneyPilotContext.InvestmentCategories.AsQueryable();
        }

        public IQueryable<ExpenseCategory> GetExpenseCategories()
        {
            return moneyPilotContext.ExpenseCategories.AsQueryable();
        }
    }
}
