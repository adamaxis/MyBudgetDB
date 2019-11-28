using System;
using System.Collections.Generic;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        public double InitAmount { get; set; }
        public DateTime DateUsrAdded { get; set; }

        public IEnumerable<Item> Expenses { get; set; }

        public class Item
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public DateTime DateAdded { get; set; }
        }
    }
}
