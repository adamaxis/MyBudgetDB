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
        
        public void CreateBudget(CreateUserBudgetCommand cmd)
        {
            var budget = cmd.ToUserBudget();
            _context.Add(budget);
            _context.SaveChanges();
            //return budget.Id;
        }
    }
}
