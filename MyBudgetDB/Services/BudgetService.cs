using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Authorization;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Models.FilterModels;
using Newtonsoft.Json;

namespace MyBudgetDB.Services
{
    public class BudgetService
    {
        readonly ApplicationDbContext _context;
        readonly ILogger _log;
        readonly UserManager<ApplicationUser> userService;
        public BudgetService(ApplicationDbContext context, ILoggerFactory factory, UserManager<ApplicationUser> _userService)
        {
            _context = context;
            _log = factory.CreateLogger<BudgetService>();
            userService = _userService;
        }

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateAccessLog(LogRequestModel log)
        {
            _log.LogInformation($"accessLog:{JsonConvert.SerializeObject(log)}");
        }

        public void CreateErrorLog(LogErrorModel log)
        {
            _log.LogError($"errorLog:{JsonConvert.SerializeObject(log)}");
        }

        public void DoLog(LogLevel lvl, string msg)
        {
            _log.Log(lvl, msg);
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
                    LastModified = x.LastModified,
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
                .Where(x => x.BudgetId == id)
                .SingleOrDefault(x => x.BudgetId == id);
        }

        public ICollection<UserBudget> GetBudgets(string id)
        {
            return _context.Budgets
                .Where(x => !x.IsDeleted)
                .Where(x => x.UserId == id)
                .ToList();
        }

        public ICollection<UserBudgetBrief> GetBudgetsBrief(string id, bool isAdmin = false)
        {
            return _context.Budgets
                .Where(r => !r.IsDeleted)
                .Where(r => (isAdmin ? r.UserId != null : r.UserId == id))
                .Select(x => new UserBudgetBrief
                {
                    Id = x.BudgetId,
                    Name = x.Name,
                    Owner = x.Owner,
                    CreationDate = x.CreationDate,
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
                    Name = x.Name,
                    Owner = x.Owner,
                    Amount = x.Amount,
                    CreationDate = x.CreationDate,
                    BudgetId = x.BudgetId,
                    UserId = x.UserId,
                    Expenses = x.Expenses.ToList(),
                    Balance = (x.Expenses.Any() ? x.Amount - x.Expenses.Sum(y => y.Amount) : 0)
                }).SingleOrDefault();
        }

        public void UpdateBudget(UpdateBudgetCommand cmd)
        {
            var budget = _context.Budgets.Find(cmd.BudgetId);
            if (budget == null) { throw new Exception("Unable to find the budget list"); }
            if (budget.IsDeleted) { throw new Exception("Unable to update a deleted budget list"); }

            _context.Entry(budget).State = EntityState.Detached;
            cmd.UpdateBudget(budget);
            InsertOrUpdateBudget(budget);
        }

        public void InsertOrUpdateBudget(UserBudget budget)
        {
            var existingBudget = _context.Budgets
                .Where(b => b.BudgetId == budget.BudgetId)
                .Include(b => b.Expenses)
                .SingleOrDefault();
            if (existingBudget == null)
            {
                _context.Add(budget);
                _context.SaveChanges();
            }
            else
            {
                _context.Entry(existingBudget).CurrentValues.SetValues(budget);
                // Delete children
                foreach (var existingExpense in existingBudget.Expenses.ToList())
                {
                    if (budget.Expenses == null || !budget.Expenses.Any(c => c.IdExpense == existingExpense.IdExpense))
                        _context.Expenses.Remove(existingExpense);
                }
                // Add/update
                if(budget.Expenses != null) foreach (var expense in budget.Expenses)
                {
                    var existingExpense = existingBudget.Expenses
                        .Where(b => b.IdExpense == expense.IdExpense)
                        .SingleOrDefault();

                    if (existingExpense != null)
                    {
                        _context.Entry(existingExpense).CurrentValues.SetValues(expense);
                    }
                    else
                    {
                        // set id to 0 for DB autoassign
                        expense.IdExpense = 0;
                        existingBudget.Expenses.Add(expense);
                        /* Design flaw in our DB
                         * Workaround requires 1 save per write
                         */
                        _context.SaveChanges();
                    }
                }
                _context.SaveChanges();
            }
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
            if (budget == null) { throw new Exception("Budget doesn't exist");}
            _context.Budgets.Remove(budget);
            _context.SaveChanges();
        }
    }
}
