using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyBudgetDB.Data;
using MyBudgetDB.Services;

namespace MyBudgetDB.Attributes
{
    public class ValidateUserFilter : IActionFilter
    {
        private readonly DbContext _demoContext;

        private readonly UserService _userService;
        private bool userHasUserName;

        public ValidateUserFilter(ApplicationDbContext _context, UserService userService)
        {
            _demoContext = _context;
            _userService = userService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Nothing to do here
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the userId from the argument added in the url
            var id = (string)context.ActionArguments["userId"];
            // Get the userName from the argument added in the url
            var userName = context.ActionArguments["userName"];
            // Get all users from the database
            var users = _userService.GetUsers();

            // Loop all users to find url user in database
            foreach (var user in users)
            {
                if (user.UserName.Equals(userName))
                {
                    // if user was found set value to true
                    userHasUserName = true;
                }
            }

            // If the userName or id typed in the url are not found set result to not found
            if (!_userService.DoesUserExist(id) || !userHasUserName)
            {
                context.Result = new NotFoundResult();
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
