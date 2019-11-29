using System;
using Microsoft.AspNetCore.Identity;

namespace MyBudgetDB.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser //table found as dbo.AspNetUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}