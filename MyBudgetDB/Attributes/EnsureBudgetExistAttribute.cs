using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBudgetDB.Services;

namespace MyBudgetDB.Attributes
{
    public class EnsureBudgetExistAttribute : TypeFilterAttribute
    {
        public EnsureBudgetExistAttribute() : base(typeof(EnsureBudgetExistsFilter)) { }

        public class EnsureBudgetExistsFilter : IActionFilter
        {
            private readonly BudgetService _service;
            public EnsureBudgetExistsFilter(BudgetService service)
            {
                _service = service;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var recipeId = (int)context.ActionArguments["id"];
                if (!_service.DoesBudgetExist(recipeId))
                {
                    context.Result = new NotFoundResult();
                }
            }

            public void OnActionExecuted(ActionExecutedContext context) { }
        }
    }
}
