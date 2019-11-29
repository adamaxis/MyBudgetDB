using System.Collections.Generic;
using System.Linq;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateBudgetCommand : EditBudgetBase
    {
        public IList<CreateExpenseCommand> _Expenses { get; set; } = new List<CreateExpenseCommand>();

        public UserBudget ToBudget(ApplicationUser createdBy)
        {
            return new UserBudget
            {
                InitAmount = Amount,
                CreationDate = DateAdded,
                Expenses = _Expenses?.Select(x => x.ToExpense()).ToList(),
                UserId = createdBy.Id,
            };
        }
    }
}
