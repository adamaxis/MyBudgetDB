using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;
using MyBudgetDB.Migrations;
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
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                service.DeleteBudget(1);
                context.SaveChanges();

                var budgetIsDeleted = service.GetBudgetDetail(1);

                Assert.Null(budgetIsDeleted);
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
                context.Budgets.Add(new UserBudget {BudgetId = 1, Name = "Budget1"});
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

                Assert.NotNull(budget);
                Assert.Equal(1, budget.BudgetId);
                Assert.Equal("Sofia", budget.Owner);
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
                    new UserBudget { BudgetId = 1, Name = "General_house", UserId = "123"},
                    new UserBudget { BudgetId = 2, Name = "Traveling", Amount = 700, UserId = "123"});
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var service = new BudgetService(context);
                var budgets = service.GetBudgets("123");

                Assert.NotNull(budgets);
                Assert.Equal(2, budgets.Count);
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
        public void UpdateBudget_CanUpdate()
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

                var toUpdate = service.GetBudgetForUpdate(1);
                toUpdate.Owner = "Bob";
                toUpdate.BudgetId = 5;
                toUpdate.Amount = 500;
                service.UpdateBudget(toUpdate);
                context.SaveChanges();

                //Assert.Equal(1, budget.BudgetId);
                //Assert.Equal("Sofia", budget.Owner);
            }
        }

        //[Fact]
        //public void GetBudgetForUpdate_CanLoadUpdateBudgetCommand()
        //{
        //    var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseSqlite(connection)
        //        .Options;

        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        context.Database.EnsureCreated();
        //        context.Budgets.AddRange(
        //            new UserBudget { BudgetId = 1, Owner = "Sofia" },
        //            new UserBudget { BudgetId = 2, Name = "Camilla" });
        //        context.SaveChanges();
        //    }

        //    // Use a separate instance of the context to verify correct data was saved to database
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        var service = new BudgetService(context);

        //        UpdateBudgetCommand budget = service.GetBudgetForUpdate(1);
        //        //budget.BudgetId = 1;
        //        //budget.Owner = "Bob";
        //        //context.SaveChanges();

        //        Assert.Equal(1, budget.BudgetId);
        //        Assert.Equal("Sofia", budget.Owner);
        //    }
        //}
    }
}
