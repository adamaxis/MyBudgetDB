using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetDetails
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        [Display(Name = "Budget Name")]
        public string Name { get; set; }
        [Display(Name = "Budget Amount"), DisplayFormat(DataFormatString = "{0:C}")]
        public double Amount { get; set; }
        [Display(Name = "Current Balance"), DisplayFormat(DataFormatString = "{0:C}")]
        public double Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModified { get; set; }
        public bool CanEditBudget { get; set; }
        public IEnumerable<Item> Expenses { get; set; }

        public class Item
        {
            public string Name { get; set; }
            [Display(Name = "Expense Cost"), DisplayFormat(DataFormatString = "{0:C}")]
            public double Amount { get; set; }
            [Display(Name = "Notes on Expense")]
            public string Notes { get; set; }
            public DateTime? DateAdded { get; set; }
        }

        public double GetBalance()
        {
            if (Expenses == null) return 0.0;
            double bal = Amount;
            foreach (Item e in Expenses) bal -= e.Amount;
            return bal;
        }

    }
}
