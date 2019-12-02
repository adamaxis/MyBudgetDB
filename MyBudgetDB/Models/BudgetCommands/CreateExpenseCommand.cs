using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateExpenseCommand : EditExpenseBase
    {
        public Expense ToExpense()
        {
            return new Expense
            {
                Name = Name,
                Amount = Amount,
                DateAdded = DateAdded
            };
        }
    }
}
