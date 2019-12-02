using System;
using System.Collections.Generic;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UpdateBudgetCommand : EditBudgetBase
    {
        public int BudgetId { get; set; }
        public string UserId { get; set; }
        public IList<Expense> Expenses { get; set; } 
            //= new List<Expense>();

        public void UpdateBudget(UserBudget budget)
        {
            budget.Name = Name;
            budget.Owner = Owner;
            budget.Balance = Balance;
            budget.CreationDate = CreationDate;
            budget.Amount = Amount;
            budget.BudgetId = BudgetId;
            budget.Expenses = Expenses;	
            budget.UserId = UserId;
            //budget.IsDeleted = IsDeleted;
            foreach (var expense in budget.Expenses)
            {
                
            }
            //budget.UserId = UserId;
            //budget.BudgetId = BudgetId;
            //budget.Expenses = x.Expenses.FirstOrDefault(o => o. == myValue);
            //foreach (var expense in budget.Expenses)
            //{
            //    expense.Amount = Amount;
            //    expense.Name = Name;
            //    expense.DateAdded = DateTime.Today;
            //}
        }
    }
}
