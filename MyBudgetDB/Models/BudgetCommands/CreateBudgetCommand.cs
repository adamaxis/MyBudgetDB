using System.Linq;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateBudgetCommand : EditBudgetBase
    {
        public UserBudget ToBudget(ApplicationUser createdBy)
        {
            return new UserBudget
            {
                InitAmount = Amount,
                CreationDate = DateAdded,
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList(),
                UserId = createdBy.Id,
            };
        }
    }
}
