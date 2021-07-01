using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CatalogoJogosAPI.Middlewares
{
    public class TratamentoExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public TratamentoExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await next(context);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = "Ocorreu um erro durante a solicitação." });
        }
    }
}
