﻿using System;
using MyBudgetDB.Data;

namespace MyBudgetDB.Models.BudgetCommands
{
    public class UpdateBudgetCommand : EditBudgetBase
    {
        public int BudgetId { get; set; }
        public string UserId { get; set; }

        public void UpdateBudget(UserBudget budget)
        {
            budget.InitAmount = InitAmount;
            budget.CreationDate = DateTime.Today;

            
            //foreach (var expense in budget.Expenses)
            //{
            //    expense.Amount = Amount;
            //    expense.Name = Name;
            //    expense.DateAdded = DateTime.Today;
            //}
        }
    }
}