using Books.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Books.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHnalder(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";


                    var contentFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contentFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            Status = context.Response.StatusCode,
                            Title = "Internal Server Error",
                            Detail = "Internal Server Error"
                        }.ToString());
                    }

                });

            });
        }
    }
}
