using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;

namespace MyBudgetDB.Services
{
    public class BudgetService
    {
        readonly ApplicationDbContext _context;
        readonly ILogger _logger;

        public BudgetService(ApplicationDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<BudgetService>();
        }

        // add delete field to db
        //public bool DoesUsrBugetExist(int id)
        //{
        //    return _context.Budgets
        //        .Where(r => !r.IsDeleted)
        //        .Where(r => r.BudgetId == id)
        //        .Any();
        //}

        public UserBudgetDetails GetBudgetDetail(int id)
        {
            return _context.Budgets
                .Where(x => x.BudgetId == id)
                //.Where(x => !x.IsDeleted)
                .Select(x => new UserBudgetDetails
                {
                    Id = x.BudgetId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateUsrAdded = x.DateUsrAdded,
                    InitAmount = x.InitAmount,
                    
                    Expenses = x.Expenses
                        .Select(item => new UserBudgetDetails.Item
                        {
                            Name = item.Name,
                            Amount = item.Amount,
                            //DateAdded = item.DateAdded
                        })
                })
                .SingleOrDefault();
        }

        public UserBudget GetBudget(int id)
        {
            return _context.Budgets
                .SingleOrDefault(x => x.BudgetId == id);
        }

        public UpdateUserBudgetCommand GetBudgetForUpdate(int id)
        {
            return _context.Budgets
                .Where(x => x.BudgetId == id)
                //.Where(x => !x.IsDeleted)
                .Select(x => new UpdateUserBudgetCommand
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateUsrAdded = x.DateUsrAdded,
                    InitAmount = x.InitAmount,
                    DateOfBirth = x.DateOfBirth
                }) .SingleOrDefault();
        }

        public void UpdateBudget(UpdateUserBudgetCommand cmd)
        {
            var budget = _context.Budgets.Find(cmd.Id);
            if (budget == null) { throw new Exception("Unable to find the recipe"); }
            //if (recipe.IsDeleted) { throw new Exception("Unable to update a deleted recipe"); }

            cmd.UpdateUserBudget(budget);
            _context.SaveChanges();
        }

        public void CreateBudget(CreateUserBudgetCommand cmd)
        {
            var budget = cmd.ToUserBudget();
            _context.Add(budget);
            _context.SaveChanges();
            //return budget.Id;
        }
    }
}
