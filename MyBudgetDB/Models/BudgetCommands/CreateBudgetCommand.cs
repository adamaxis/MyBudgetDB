using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateBudgetCommand : EditBudgetBase
    {
        public UserBudget ToBudget()
        {
            return new UserBudget
            {
                InitAmount = Amount,
                CreationDate = DateAdded,
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList()
            };
        }
    }
}
