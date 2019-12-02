using MyBudgetDB.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class EditBudgetBase
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(35)]
        public string Owner { get; set; }

        [Range(0, double.MaxValue)]
        public double Balance { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Range(0, double.MaxValue)]
        public double Amount { get; set; }
    }
}
