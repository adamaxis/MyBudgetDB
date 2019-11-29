using System;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Data
{
    public class Expense
    {
        [Key]
        public int IdExpense { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public DateTime? DateAdded { get; set; }  // DateTime ....
    }
}