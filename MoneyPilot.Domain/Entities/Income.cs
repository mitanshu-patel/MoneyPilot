using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Entities
{
    public class Income
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateOnly CreditDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int AccountId { get; set; }

        public BankAccount Account { get; set; }
    }
}
