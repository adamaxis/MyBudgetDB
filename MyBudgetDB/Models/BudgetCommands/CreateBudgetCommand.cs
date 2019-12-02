using System;
using System.Collections.Generic;
using System.Linq;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateBudgetCommand : EditBudgetBase
    {
        public IList<CreateExpenseCommand> Expenses { get; set; } = new List<CreateExpenseCommand>();

        public UserBudget ToBudget(ApplicationUser createdBy)
        {
            return new UserBudget
            {
                Amount = Amount,
                Name = Name,
                Owner = createdBy.Email,
                UserId = createdBy.Id,
                CreationDate = DateTime.Now,
                Balance = Amount - Expenses.Sum(x => x.Amount),
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList()
            };
        }
    }
}
