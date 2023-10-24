using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace TektonLabs.Presentation.Api.Helpers.ErrorHandling
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception == null)
            {
                return;
            }

            int errorCode = (int)HttpStatusCode.InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Status = errorCode,
                Title = context.Exception.Message,
                Instance = context.HttpContext.Request.Path
            };

            context.HttpContext.Response.ContentType = "application/problem+json";
            ObjectResult result = new ObjectResult(problemDetails);
            result.StatusCode = errorCode;
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
