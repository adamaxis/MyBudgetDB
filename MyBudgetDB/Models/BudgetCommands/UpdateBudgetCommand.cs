using System;
using System.Collections.Generic;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UpdateBudgetCommand : EditBudgetBase
    {
        //public UpdateBudgetCommand(ICollection<Expense> expenses)
        //{
        //    Expenses = expenses;
        //}
        //public ICollection<Expense> Expenses { get; set; }

        public int BudgetId { get; set; }
        public string UserId { get; set; }

        public void UpdateBudget(UserBudget budget)
        {
            budget.Amount = Amount;
            budget.CreationDate = DateTime.Today;

            
            //foreach (var expense in budget.Expenses)
            //{
            //    expense.Amount = Amount;
            //    expense.Name = Name;
            //    expense.DateAdded = DateTime.Today;
            //}
        }
    }
}
