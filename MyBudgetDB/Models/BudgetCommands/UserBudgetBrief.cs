using System;
using System.Collections.Generic;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UserBudgetBrief
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance;
    }
}
