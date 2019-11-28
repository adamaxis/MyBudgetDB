using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UpdateUserBudgetCommand : EditUserBudgetBase
    {
        public int Id { get; set; }

        public void UpdateUserBudget(UserBudget budget)
        {
            budget.FirstName = FirstName;
            budget.LastName = LastName;
            budget.InitAmount = InitAmount;
            budget.DateUsrAdded = DateUsrAdded;
            budget.DateOfBirth = DateOfBirth;
            //Email = Email,

            foreach (var expense in budget.Expenses)
            {
                expense.Amount = Amount;
                expense.Name = Name;
                expense.DateAdded = DateUsrAdded;
            }
        }
    }
}
