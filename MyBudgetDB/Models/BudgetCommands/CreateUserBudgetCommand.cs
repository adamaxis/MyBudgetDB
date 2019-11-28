using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateUserBudgetCommand : EditUserBudgetBase
    {
        //public IList<CreateExpenseCommand> Expenses { get; set; } = new List<CreateExpenseCommand>();

        public UserBudget ToUserBudget()
        {
            return new UserBudget
            {
                FirstName = FirstName,
                LastName = LastName,
                InitAmount = InitAmount,
                DateUsrAdded = DateUsrAdded,
                DateOfBirth = DateOfBirth,
                //Email = Email,
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList()
            };
        }
    }
}
