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

        public Task DeleteAccountAsync(BankAccount bankAccount)
        {
            moneyPilotContext.BankAccounts.Remove(bankAccount);
            return moneyPilotContext.SaveChangesAsync();
        }

        public IQueryable<Investment> GetInvestments()
        {
           return moneyPilotContext.Investments.AsQueryable();
        }

        public Task SaveInvestmentAsync(Investment investment)
        {
            if(investment.Id == 0)
            {
                moneyPilotContext.Investments.Add(investment);
            }
            return moneyPilotContext.SaveChangesAsync();
        }

        public async Task DeleteInvestmentAsync(Investment investment)
        {
            moneyPilotContext.Investments.Remove(investment);
            await moneyPilotContext.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            moneyPilotContext.Expenses.Remove(expense);
            await moneyPilotContext.SaveChangesAsync();
        }
    }
}
