using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Data
{
    public class UserBudget
    {
        [Key]
        public int BudgetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; } // I think we don't need to add this one as is already in IdentityUser
        public double InitAmount { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateUsrAdded { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
