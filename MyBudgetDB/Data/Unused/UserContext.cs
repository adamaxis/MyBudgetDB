using CIS174_TestCoreApp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Entities.Budget;

namespace CIS174_TestCoreApp
{
    public class UserContext 
        //: IdentityDbContext<AppUser>
    {
        //public UserContext(DbContextOptions<UserContext> options) : base(options)
        //{

        //}
         
        //public DbSet<Expense> Expenses { get; set; }

        //public DbSet<UserClass> People { get; set; }
       // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(@"Server=tcp:mybudgetdb.database.windows.net,1433;Initial Catalog=budget;Persist Security Info=False;User ID=ThreeMusketeers;Password=Musketeers19!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    }
}
