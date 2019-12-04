using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBudgetDB.Services;
using Microsoft.Extensions.Logging;

namespace MyBudgetDB.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var service = (BudgetService)context.HttpContext.RequestServices.GetService(typeof(BudgetService));
                service.DoLog(LogLevel.Warning, $"{context.HttpContext.User} failed to validate model on {context.Controller}");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
