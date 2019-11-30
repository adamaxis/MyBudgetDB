using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;
using Xunit;

namespace XUnitTestMyBudgetDB
{
    public class TestBudgetService
    {
        [Fact]
        public async Task CreateBudget_CreatesCorrectly()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            const string budgetName = "General budget";
            const double amount = 1000.0;

            // Run the test against one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                var service = new BudgetService(context);
                var cmd = new CreateBudgetCommand
                {
                    Amount = amount,
                    Name = budgetName,
                };
                var user = new ApplicationUser
                {
                    Id = 123.ToString()
                };
                var budgetId = service.CreateBudget(cmd, user);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.Budgets.CountAsync());

                var budget = await context.Budgets.SingleAsync();
                Assert.Equal(amount, budget.Amount);
                Assert.Equal(budgetName, budget.Name);
            }
        }
    }
}
