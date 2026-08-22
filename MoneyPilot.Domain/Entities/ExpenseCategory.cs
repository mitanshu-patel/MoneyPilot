using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Entities
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public bool HasAutoPayment { get; set; } = false;

        public List<Expense> Expenses { get; set; }
    }
}
