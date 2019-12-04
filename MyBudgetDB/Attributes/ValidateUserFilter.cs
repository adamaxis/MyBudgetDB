using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Data;

namespace MyBudgetDB.Attributes
{
    public class ValidateUserFilter : IActionFilter
    {
        private readonly DbContext _demoContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _log;

        public ValidateUserFilter(ApplicationDbContext _context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ValidateUserFilter> log)
        {
            _demoContext = _context;
            _userManager = userManager;
            _signInManager = signInManager;
            _log = log;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Nothing to do here
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            //var user = _userManager.GetUserAsync(_user);
            if (user == null)
            {
                _log.LogWarning($"Unable to load user with ID '{context.HttpContext.User}'");
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public class ValidateUserAttribute : TypeFilterAttribute
    {
        public ValidateUserAttribute()
            : base(typeof(ValidateUserFilter))
        {

        }
    }
}
