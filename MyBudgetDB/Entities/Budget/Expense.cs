using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Entities.Budget
{
    public class Expense
    {
        [Key]
        public int IdExpense { get; set; }

        public string Name { get; set; }
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}