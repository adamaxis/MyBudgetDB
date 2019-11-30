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

        [Fact]
        public void GetBudgetDetails_CanLoadFromContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Budgets.AddRange(
                    new UserBudget { BudgetId = 1, Name = "General_house" },
                    new UserBudget { BudgetId = 2, Name = "Traveling" },
                    new UserBudget { BudgetId = 3, Name = "Camilla" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new  BudgetService(context);

                var budget = service.GetBudgetDetail(2);

                Assert.NotNull(budget);
                Assert.Equal(2, budget.Id);
                Assert.Equal("Traveling", budget.Name);
            }
        }

        [Fact]
        public void GetBudgetDetails_DoesNotLoadDeletedBudget()
        {
            const string budgetName = "Test Budget";
            const int budgetId = 2;

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Budgets.Add(new UserBudget { BudgetId  = 1, Name = "Budget1" });
                context.Budgets.Add(new UserBudget { BudgetId = 2, Name = budgetName, IsDeleted = true });
                context.Budgets.Add(new UserBudget { BudgetId = 3, Name = "Budget3" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);

                var budget = service.GetBudgetDetail(budgetId);

                Assert.Null(budget);
            }
        }

    }
}
