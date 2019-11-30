using System;
using System.Collections.Generic;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetBrief
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public double Amount { get; set; } // not getting initial amount
        public double Balance { get; set; }
        public ICollection<Expense> Expenses { get; set; } 

        //public double Balance
        //{
        //    get
        //    {
        //        double fnBalance = Expenses.Sum(expense => expense.Amount);

        //        return InitAmount - fnBalance;
        //    }
        //}
    }
}
