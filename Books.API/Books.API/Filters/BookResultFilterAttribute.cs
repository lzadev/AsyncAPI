using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Books.API.Filters
{
    public class BookResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            var resultFromAtion = context.Result as ObjectResult;

            if(resultFromAtion?.Value == null
                || resultFromAtion.StatusCode < 200
                || resultFromAtion.StatusCode >= 300)
            {
                await next();

                return;
            }

            //opteniendo el servicio mapper
            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            resultFromAtion.Value = mapper.Map<Models.Book>(resultFromAtion.Value);

            //verifico si el tipo de retorno de mi accion es asignable a IEnumerable retorno un ienumerable de Models.Book
            //if (typeof(System.Collections.IEnumerable)
            //    .IsAssignableFrom(resultFromAtion.Value.GetType()))
            //{
            //    resultFromAtion.Value = mapper.Map<IEnumerable<Models.Book>>(resultFromAtion.Value);
            //}
            //else
            //{
            //    resultFromAtion.Value = mapper.Map<Models.Book>(resultFromAtion.Value);
            //}

            await next();
        }
    }
}
