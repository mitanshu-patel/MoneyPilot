using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int? AutoDebitDay { get; set; } // in case of EMIs, subscriptions, mutual funds, SIPs, etc.

        public decimal Amount { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int? AccountId { get; set; }

        public BankAccount? Account { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public List<Expense> Expenses { get; set; } = [];

        public List<Investment> Investments { get; set; } = [];
    }
}
