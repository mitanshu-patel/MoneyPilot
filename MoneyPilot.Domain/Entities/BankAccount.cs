using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }

        public string HolderName { get; set; }

        public decimal Balance { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<Expense> Expenses { get; set; } = [];

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
