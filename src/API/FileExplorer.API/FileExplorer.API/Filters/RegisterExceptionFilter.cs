using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileExplorer.API
{
    public class RegisterExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Register failed!",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Email existed",
            };

            context.Result = new ObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }
}
