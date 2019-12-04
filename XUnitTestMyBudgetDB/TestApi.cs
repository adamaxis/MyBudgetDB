using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;
using Xunit;

namespace XUnitTestMyBudgetDB
{
    public class TestApi
    {
        [Fact]
        public void GetBudget_ShouldReturnBudgetDetail()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            const string budgetName = "General budget";
            const double amount = 1000.0;
            
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

                // Test to get value with Api
                //BudgetApiController controller = new BudgetApiController();
                //var result = controller.GetBudget(budgetId);
            }

           

            //Assert.AreEqual(testProducts.Count, result.Count);
        }
    }
}
