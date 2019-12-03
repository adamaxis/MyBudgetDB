using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBudgetDB.Models.FilterModels;
using MyBudgetDB.Services;
using System;

namespace MyBudgetDB.Attributes
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var error = new
            {
                Success = false,
                Errors = new[] { context.Exception.Message }
            };

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };

            var StatusCode = context.HttpContext.Response.StatusCode;
            LogErrorModel errorLog = new LogErrorModel
            {
                StatusCode = StatusCode,
                RequestId = context.HttpContext.Response.Headers["requestId"],
                ExceptionMessage = context.Exception.Message,
                StackTrace = context.Exception.StackTrace,
                TimeOfError = DateTime.Now
            };
            var service = (BudgetService)context.HttpContext.RequestServices.GetService(typeof(BudgetService));
            service.CreateErrorLog(errorLog);
            //context.Result = new JsonResult(errorLog);
            context.ExceptionHandled = true;
        }
    }
}
