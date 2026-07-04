using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public Transaction Transaction { get; set; }
        public int CategoryId { get; set; }

        public ExpenseCategory Category { get; set; }
    }
}
