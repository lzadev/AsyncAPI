using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Filters
{
    public class BooksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAtion = context.Result as ObjectResult;

            if (resultFromAtion?.Value == null
                || resultFromAtion.StatusCode < 200
                || resultFromAtion.StatusCode >= 300)
            {
                await next();

                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            resultFromAtion.Value = mapper.Map<IEnumerable<Models.Book>>(resultFromAtion.Value);

            await next();
        }
    }
}
