using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class EditBudgetBase
    {
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public double Balance { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        //public IList<CreateExpenseCommand> Expenses { get; set; } = new List<CreateExpenseCommand>();

        [Range(0, double.MaxValue)]
        public double InitAmount { get; set; }

        //public bool IsDeleted { get; set; }
    }
}
