using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PatientsProject.Application.Exceptions;
using System.Net;

namespace PatientsProject.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var response = new ProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An error occurred while processing your request."
            };

            switch (exception)
            {
                case NotFoundException notFoundException:
                    response.Title = notFoundException.Message;
                    response.Status = (int)HttpStatusCode.NotFound;
                    break;

                case ApplicationValidationException validationException:
                    response.Title = "Validation Error";
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Extensions.Add("errors", validationException.Errors);
                    break;
            }

            context.Result = new ObjectResult(response) { StatusCode = response.Status };

            context.ExceptionHandled = true;
        }
    }
}
