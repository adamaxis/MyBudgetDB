using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MyBudgetDB.Entities.Budget;

namespace MyBudgetDB.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser //table found as dbo.AspNetUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; } // I think we don't need to add this one as is already in IdentityUser
        public double Amount { get; set; }
        public Expense[] Expenses {get; set;}

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}