using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace my_library_cosmos_db.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Container _container; // Contêiner do Cosmos DB

        public ErrorMiddleware(RequestDelegate next, Container container)
        {
            _next = next;
            _container = container; // Injetar o contêiner do Cosmos DB
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorMiddlewareResponse errorMiddlewareResponse;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                errorMiddlewareResponse = new ErrorMiddlewareResponse(HttpStatusCode.InternalServerError.ToString(), $"{ex.Message} {ex?.InnerException?.Message}");
            }
            else
            {
                errorMiddlewareResponse = new ErrorMiddlewareResponse(HttpStatusCode.InternalServerError.ToString(), "An internal server error has occurred");
            }

            // Gravar log de erro no Cosmos DB usando o TraceId existente
            var logEntry = new ErrorLogModel
            {
                Id = Guid.NewGuid().ToString(),
                TraceId = errorMiddlewareResponse.TraceId.ToString(), // Reutilizando o TraceId
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Errors = "errors",
                InnerException = ex.InnerException?.Message,
                Timestamp = DateTime.UtcNow
            };

            var partitionKey = new PartitionKey(logEntry.Errors);

            await _container.CreateItemAsync(logEntry, partitionKey); // Salvar o log no Cosmos DB

            var result = JsonConvert.SerializeObject(errorMiddlewareResponse);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }

    // Modelo para o log de erro
    public class ErrorLogModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Errors { get; set; }

        public string TraceId { get; set; } // Reutilizando o TraceId da resposta

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string InnerException { get; set; }

        public DateTime Timestamp { get; set; }
    }

}
