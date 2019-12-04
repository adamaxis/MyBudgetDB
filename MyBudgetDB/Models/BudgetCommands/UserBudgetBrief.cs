using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetBrief
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public double Amount { get; set; } // not getting initial amount
        public ICollection<Expense> Expenses { get; set; }

        public double GetBalance()
        {
            if (Expenses.Count == 0) return 0.0;
            double bal = Amount;
            foreach (Expense e in Expenses) bal -= e.Amount;
            return bal;
        }

    }
}
