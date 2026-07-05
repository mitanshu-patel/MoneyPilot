using MoneyPilot.Domain.Entities;

namespace MoneyPilot.Application
{
    public interface IMoneyPilotRepo
    {
        public IQueryable<User> GetUsers();
        public Task<int> AddNewUser(User user);
    }
}
