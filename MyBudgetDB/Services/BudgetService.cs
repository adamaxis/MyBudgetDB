using System;
using System.Collections.Generic;
using System.Linq;
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

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        // add delete field to db
        public bool DoesBudgetExist(int id)
        {
            return _context.Budgets
                .Where(r => !r.IsDeleted)
                .Any(r => r.BudgetId == id);
        }

        public UserBudgetDetails GetBudgetDetail(int id)
        {
            return _context.Budgets
                .Where(x => x.BudgetId == id)
                .Where(x => !x.IsDeleted)
                .Select(x => new UserBudgetDetails
                {
                    Id = x.BudgetId,
                    CreationDate = x.CreationDate,
                    InitAmount = x.InitAmount,

                    Expenses = x.Expenses
                        .Select(item => new UserBudgetDetails.Item
                        {
                            Name = item.Name,
                            Amount = item.Amount,
                            DateAdded = item.DateAdded
                        })
                })
                .SingleOrDefault();
        }

        public UserBudget GetBudget(int id)
        {
            return _context.Budgets
                .SingleOrDefault(x => x.BudgetId == id);
        }

        public ICollection<UserBudget> GetBudgets(string id)
        {
            return _context.Budgets
                .Where(x => !x.IsDeleted)
                .Where(x => x.UserId == id)
                .ToList();
        }

        public ICollection<UserBudgetBrief> GetBudgetsBrief(string id)
        {
            return _context.Budgets
                .Where(r => !r.IsDeleted)
                .Select(x => new UserBudgetBrief
                {
                    Id = x.BudgetId,
                    Name = x.Name,
                    CreationDate = x.CreationDate,
                    Balance = x.Balance
                })
                .ToList();
        }

        public UpdateBudgetCommand GetBudgetForUpdate(int id)
        {
            return _context.Budgets
                .Where(x => x.BudgetId == id)
                .Where(x => !x.IsDeleted)
                .Select(x => new UpdateBudgetCommand
                {
                    InitAmount = x.InitAmount,
                    CreationDate = x.CreationDate,
                    
                }).SingleOrDefault();
        }

        public void UpdateBudget(UpdateBudgetCommand cmd)
        {
            var budget = _context.Budgets.Find(cmd.BudgetId);
            if (budget == null) { throw new Exception("Unable to find the budget list"); }
            if (budget.IsDeleted) { throw new Exception("Unable to update a deleted budget list"); }

            cmd.UpdateBudget(budget);
            _context.SaveChanges();
        }

        public int CreateBudget(CreateBudgetCommand cmd, ApplicationUser createdBy)
        {
            var budget = cmd.ToBudget(createdBy);
            _context.Add(budget);
            _context.SaveChanges();
            return budget.BudgetId;
        }
    }
}
