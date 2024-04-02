using System.Net;
using System.Text.Json;

namespace BuberDinner.Api.Middlware
{
    public class ErrorHandlingMiddlware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex)
            {
                await HandleExeptionAsync(context, ex);
            }
        }

        public static Task HandleExeptionAsync(HttpContext context, Exception exception) 
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = JsonSerializer.Serialize(new {error = "An error occurred while processing your request"});

            context.Response.ContentType = "application/json"; 
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
