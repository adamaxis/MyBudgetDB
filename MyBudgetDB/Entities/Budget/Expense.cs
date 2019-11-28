using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Entities.Budget
{
    public class Expense
    {
        [Key]
        public int IdExpense { get; set; }

        public long UserId { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        [StringLength(32)]
        public string Category { get; set; }

        [StringLength(256)]
        public string Notes { get; set; }

        public double Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }  // DateTime ....
    }
}