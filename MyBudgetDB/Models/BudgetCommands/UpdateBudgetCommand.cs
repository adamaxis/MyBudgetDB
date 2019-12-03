using System.Collections.Generic;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UpdateBudgetCommand : EditBudgetBase
    {
        public int BudgetId { get; set; }
        public string UserId { get; set; }
        public IList<Expense> Expenses { get; set; } 

        public void UpdateBudget(UserBudget budget)
        {
            budget.Name = Name;
            budget.Balance = Balance;
            budget.Amount = Amount;
            budget.Expenses = Expenses;
        }
    }
}
