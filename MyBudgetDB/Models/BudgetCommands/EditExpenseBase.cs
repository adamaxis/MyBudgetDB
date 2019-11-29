using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class EditExpenseBase
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}
