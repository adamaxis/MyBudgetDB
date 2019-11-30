using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetDetails
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        [Display(Name = "Budget Amount"), DisplayFormat(DataFormatString = "{0:C}")]
        public double Amount { get; set; }
        [Display(Name = "Current Balance"), DisplayFormat(DataFormatString = "{0:C}")]
        public double Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<Item> Expenses { get; set; }

        public class Item
        {
            public string Name { get; set; }
            [Display(Name = "Expense Cost"), DisplayFormat(DataFormatString = "{0:C}")]
            public double Amount { get; set; }
            public DateTime? DateAdded { get; set; }
        }


    }
}
