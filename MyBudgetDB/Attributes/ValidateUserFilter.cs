using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;

namespace MyBudgetDB.Attributes
{
    public class ValidateUserFilter : IActionFilter
    {
        private readonly DbContext _demoContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ValidateUserFilter(ApplicationDbContext _context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _demoContext = _context;
            _userManager = userManager;
            _signInManager = signInManager;
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
