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
                    Amount = x.Amount,
                    Owner = x.Owner,
                    Name = x.Name,
                    Balance = x.Balance,
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

        public double GetBalance(int id)
        {
            var budget = _context.Budgets
                .SingleOrDefault(x => x.BudgetId == id);
            var initAmt = (double)(budget.Amount);
            var fnBalance = 0.0;

            foreach (var expense in budget.Expenses)
            {
                fnBalance += expense.Amount;
            }

            return initAmt - fnBalance;
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
                    Balance = x.Balance,
                    Amount = x.Amount,
                    Expenses = x.Expenses
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
                    Amount = x.Amount,
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

        public void DeleteBudget(int budgetId)
        {
            var budget = _context.Budgets.Find(budgetId);
            if (budget.IsDeleted) { throw new Exception("Unable to delete a deleted budget");}

            budget.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
