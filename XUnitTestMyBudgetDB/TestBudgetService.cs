using System.Collections.Generic;
using System.Linq;
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
                Assert.Equal(1000.0, budget.Amount);
                Assert.Equal("General budget", budget.Name);
                Assert.NotEqual(500, budget.Amount);
                Assert.NotNull(budget);
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

            // Insert data into the database using one instance of the context
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
                var service = new BudgetService(context);
                var budget = service.GetBudgetDetail(2);

                Assert.NotNull(budget);
                Assert.Equal(2, budget.Id);
                Assert.Equal("Traveling", budget.Name);
                Assert.NotEqual(500, budget.Amount);
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
                context.Budgets.Add(new UserBudget { BudgetId = 1, Name = "Budget1" });
                context.Budgets.Add(new UserBudget { BudgetId = 2, Name = budgetName, IsDeleted = true });
                context.Budgets.Add(new UserBudget { BudgetId = 3, Name = "Budget3" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var budgetDeleted = service.GetBudgetDetail(budgetId);
                var budgetNotDel1 = service.GetBudgetDetail(1);
                var budgetNotDel3 = service.GetBudgetDetail(3);

                Assert.Null(budgetDeleted);
                Assert.Equal("Budget1", budgetNotDel1.Name);
                Assert.Equal("Budget3", budgetNotDel3.Name);
                Assert.NotEqual("Budget3", budgetNotDel1.Name);
            }
        }

        [Fact]
        public void DeleteBudgetMarksIsDeleteTrue()
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
                context.Budgets.Add(new UserBudget { BudgetId = 1, Name = "Budget1" });
                context.Budgets.Add(new UserBudget { BudgetId = 2, Name = "Budget2" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                service.DeleteBudget(1);
                context.SaveChanges();

                var budgetIsDeleted = service.GetBudgetDetail(1);
                var budgetNotDel2 = service.GetBudgetDetail(2);

                Assert.Null(budgetIsDeleted);
                Assert.Equal("Budget2", budgetNotDel2.Name);
                Assert.NotEqual("Budget1", budgetNotDel2.Name);
            }
        }

        [Fact]
        public void TestDoesBudgetExist()
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
                context.Budgets.Add(new UserBudget { BudgetId = 1, Name = "Budget1" });
                context.SaveChanges();
            }

            // should return true when budget was persisted
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var exist = service.DoesBudgetExist(1);

                Assert.True(exist);
            }

            // should return false when budget doesn't exist
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var exist = service.DoesBudgetExist(6);

                Assert.False(exist);
            }
        }

        [Fact]
        public void GetBudget_CanLoadABudgetFromContext()
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
                    new UserBudget { BudgetId = 1, Owner = "Sofia" },
                    new UserBudget { BudgetId = 2, Name = "Camilla" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);

                var budget = service.GetBudget(1);
                var budget2 = service.GetBudget(2);

                Assert.NotNull(budget);
                Assert.Equal(1, budget.BudgetId);
                Assert.Equal("Sofia", budget.Owner);
                Assert.Equal("Camilla", budget2.Name);
                Assert.NotEqual("Cama", budget.Owner);
            }
        }

        [Fact]
        public void GetBudgets_CanLoadBudgetsFromContext()
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
                    new UserBudget { BudgetId = 1, Name = "General_house", UserId = "123" },
                    new UserBudget { BudgetId = 2, Name = "Traveling", Amount = 700, UserId = "123" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var budgets = service.GetBudgets("123");

                Assert.NotNull(budgets);
                Assert.Equal(2, budgets.Count);
                Assert.NotEqual(1, budgets.Count);
            }
        }

        [Fact]
        public void GetBudgetsBrief_CanLoadBudgetsFromContext()
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
                    new UserBudget { Name = "General_house", UserId = "234" },
                    new UserBudget { Name = "Traveling", Amount = 700, UserId = "45" });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var budgets = service.GetBudgets("45");

                Assert.NotNull(budgets);
                Assert.Equal(1, budgets.Count);
            }
        }

        [Fact]
        public void UpdateBudget_CanLoadUpdateBudgetType()
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
                    new UserBudget { BudgetId = 1, Owner = "Sofia" },
                    new UserBudget { BudgetId = 2, Name = "Camilla" });
                context.SaveChanges();
            }

            // Use a separate instance edit some data
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);

                UpdateBudgetCommand toUpdate = service.GetBudgetForUpdate(2);

                Assert.NotNull(toUpdate);
            }
        }

        [Fact]
        public void GetBudgetForUpdate_Returns_UpdateBudgetCommand_Type()
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
                    new UserBudget { BudgetId = 1, Owner = "Sofia", UserId = "123" }
                     );
                context.SaveChanges();
            }

            // Use a separate instance edit some data
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);

                UpdateBudgetCommand obj = service.GetBudgetForUpdate(1);

                Assert.NotNull(obj);
                Assert.Equal("Sofia", obj.Owner);
                Assert.Equal("123", obj.UserId);
                Assert.NotEqual("Linda", obj.Owner);
            }
        }

        [Fact]
        public void InserUpdate_CanUpdate()
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
                Expense exp1 = new Expense();
                exp1.Amount = 300;
                exp1.Name = "hotel";
                List<Expense> explist = new List<Expense>();
                explist.Add(exp1);
                context.Budgets.AddRange(
                    new UserBudget { BudgetId = 1, Owner = "Sofia" },
                    new UserBudget { BudgetId = 2, Name = "Camilla", Expenses = explist });
                context.SaveChanges();
            }

            // Use a separate instance edit some data
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);

                Expense exp1 = new Expense();
                exp1.Amount = 2;
                exp1.Name = "milk";
                List<Expense> explist = new List<Expense>();
                explist.Add(exp1);
                //explist.Add(exp2);
                var user1 = new UserBudget
                {
                    BudgetId = 1,
                    Owner = "Bob",
                    Expenses = explist

                };

                service.InsertOrUpdateBudget(user1);

                var budget = service.GetBudget(1);
                var expenses = budget.Expenses.ToArray();

                Assert.NotNull(budget);
                Assert.NotNull(budget.Expenses);
                Assert.Equal("milk", expenses[0].Name);
                Assert.NotEqual("Sofia", budget.Owner);
                Assert.Equal("Bob", budget.Owner);
            }
        }
    }
}
