using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class EditUserBudgetBase : EditExpenseBase
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        //[Required, StringLength(50)]
        //public string Email { get; set; }

        [Range(0, double.MaxValue)]
        public double InitAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateUsrAdded { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public IList<CreateExpenseCommand> Expenses { get; set; } = new List<CreateExpenseCommand>();
    }
}
