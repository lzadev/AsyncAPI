using Books.API.Exceptions;
using Books.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Books.API.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(LogicException))
            {
                var response = new
                {
                    errors = new[]
                    {
                        new ErrorDetails
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Title = "Bad Request",
                            Detail = context.Exception.Message
                        }
                    }
                };

                context.Result = new BadRequestObjectResult(response);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            }
        }
    }
}
