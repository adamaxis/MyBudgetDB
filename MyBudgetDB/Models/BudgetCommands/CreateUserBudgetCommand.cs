using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MyBudgetDB.Entities.Budget;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateUserBudgetCommand
    {
        //Edit user
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }

        
        public string Date { get; set; }
        
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
        //End user
        
        public IList<CreateExpenseCommand> Expenses { get; set; } = new List<CreateExpenseCommand>();

        public ApplicationUser ToUserBudget()
        {
            return new ApplicationUser
            {
                FirstName = FirstName,
                LastName = LastName,
                Amount = Amount,
                DateAdded = DateAdded,
                Email = Email,
                Expenses = Expenses?.Select(x => x.ToExpense()).ToList()
            };
        }
    }
}
