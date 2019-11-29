using System.Collections.Generic;
using System.Linq;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateBudgetCommand : EditBudgetBase
    {
        public IList<CreateExpenseCommand> expenses { get; set; } = new List<CreateExpenseCommand>();

        public UserBudget ToBudget(ApplicationUser createdBy)
        {
            return new UserBudget
            {
                InitAmount = InitAmount,
                Name = Name,
                CreationDate = CreationDate,
                Balance = InitAmount, // this needs to be initialamount - each expense amount
                Expenses = expenses?.Select(x => x.ToExpense()).ToList(),
                UserId = createdBy.Id,
            };
        }
    }
}
