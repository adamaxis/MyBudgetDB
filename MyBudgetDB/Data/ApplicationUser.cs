using System;
using Microsoft.AspNetCore.Identity;

namespace MyBudgetDB.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser //table found as dbo.AspNetUsers
    {
        //[StringLength(32)]
        public string FirstName { get; set; }
        //[StringLength(32)]
        public string LastName { get; set; }
        ////public string Email { get; set; } // I think we don't need to add this one as is already in IdentityUser
        //public double Amount { get; set; }
        //public List<Expense> Expenses {get; set;}

        //[Display(Name = "Date of Birth")]
        //[DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        //[DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}