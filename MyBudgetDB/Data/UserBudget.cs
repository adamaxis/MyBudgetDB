using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Data
{
    public class UserBudget
    {
        [Key]
        public int BudgetId { get; set; }

        public string UserId { get; set; }

        public string Owner { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public double Balance { get; set; }
       
        public DateTime CreationDate { get; set; }

        public ICollection<Expense> Expenses { get; set; }

        public bool IsDeleted { get; set; }

    }
}