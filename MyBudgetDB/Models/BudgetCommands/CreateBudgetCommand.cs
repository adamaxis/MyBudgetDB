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
                InitAmount = InitAmount,
                Name = Name,
                CreationDate = CreationDate,
                Balance = InitAmount,
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList(),
                UserId = createdBy.Id,
            };
        }
    }
}
