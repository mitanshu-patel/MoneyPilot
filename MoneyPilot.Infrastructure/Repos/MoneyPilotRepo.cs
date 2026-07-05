using MoneyPilot.Application;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.Repos
{
    public class MoneyPilotRepo(MoneyPilotContext moneyPilotContext) : IMoneyPilotRepo
    {
        public async Task<int> AddNewUser(User user)
        {
           moneyPilotContext.Users.Add(user);
           await moneyPilotContext.SaveChangesAsync();
           return user.Id;
        }

        public IQueryable<User> GetUsers()
        {
            return moneyPilotContext.Users.AsQueryable();
        }
    }
}
