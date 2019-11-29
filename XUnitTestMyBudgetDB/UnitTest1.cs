using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;
using Xunit;


// happy path, erro path, edge cases
namespace XUnitTestMyBudgetDB
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public async Task CreateBudget_CreatesCorrectly()
        {
            const double amount = 2000.0;
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureCreated();

                    var service = new BudgetService(context);
                    var cmd = new CreateBudgetCommand
                    {
                        InitAmount = amount,
                    };
                    var user = new ApplicationUser
                    {
                        Id = 123.ToString(),
                    };
                    var budgetId = service.CreateBudget(cmd, user);
                    context.SaveChanges();
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new ApplicationDbContext(options))
                {
                    Assert.Equal(1, await context.Budgets.CountAsync());

                    var budget = await context.Budgets.SingleAsync();
                    Assert.Equal(amount, budget.InitAmount);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
