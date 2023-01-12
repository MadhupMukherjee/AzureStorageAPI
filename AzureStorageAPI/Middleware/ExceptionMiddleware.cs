using AzureStorageAPI.Model;
using Newtonsoft.Json;
using System.Net;

namespace AzureStorageAPI.Middleware
{


    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorDetails error = new ErrorDetails();

            if (exception is NullReferenceException nullReferenceException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                error.StatusCode = context.Response.StatusCode;
                error.message = nullReferenceException.Message;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }

            else if (exception is FileNotFoundException fileNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                error.StatusCode = context.Response.StatusCode;
                error.message = fileNotFoundException.Message;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
            else if (exception is InvalidOperationException invalidOperationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                error.StatusCode = context.Response.StatusCode;
                error.message = invalidOperationException.Message;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error.StatusCode = context.Response.StatusCode;
                error.message = exception.Message;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
        }
    }
}
