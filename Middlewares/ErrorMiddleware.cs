using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;

namespace my_library_cosmos_db.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //gravar log de erro com traceId


            ErrorMiddlewareResponse errorMiddlewareResponse;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") == "Development")
            {
                errorMiddlewareResponse = new ErrorMiddlewareResponse(HttpStatusCode.InternalServerError.ToString(), $"{ex.Message} {ex?.InnerException?.Message}");
            }
            else
            {
                errorMiddlewareResponse = new ErrorMiddlewareResponse(HttpStatusCode.InternalServerError.ToString(), "An internal server error has ocurred");
            }

            var result = JsonConvert.SerializeObject(errorMiddlewareResponse);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
