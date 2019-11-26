using System;
using System.ComponentModel.DataAnnotations;
using MyBudgetDB.Entities.Budget;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class CreateExpenseCommand
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        
        public double Amount { get; set; }
        
        public DateTime DateAdded { get; set; }


        public Expense ToExpense()
        {
            return new Expense
            {
                Name = Name,
                Amount = Amount,
                DateAdded = DateAdded // Fix
            };
        }
    }
}
