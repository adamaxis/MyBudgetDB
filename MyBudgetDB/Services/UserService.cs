using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Data;
using MyBudgetDB.Models;

namespace MyBudgetDB.Services
{
    public class UserService
    {
        readonly ApplicationDbContext _context;
        readonly ILogger _logger;
        public UserService(ApplicationDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<BudgetService>();
        }

        public string CreateUser(CreateUser cmd)
        {
            var user = cmd.ToUser();
            _context.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public bool DoesUserExist(string id)
        {
            //var user = _context.Users.SingleOrDefault(x => x.Username == username);
            var user = _context.Users.Find(id);

            // check if username exists
            if (user == null) return false;

            return true;
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users
                .Select(x => new ApplicationUser()
                {
                    Id = x.Id,
                    UserName = x.UserName
                })
                .ToList();
        }
    }
}
