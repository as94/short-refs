namespace ShortRefs.Api.Pipelines
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    using ShortRefs.Api.Exceptions;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ForbiddenException)
            {
                code = HttpStatusCode.Unauthorized;
            }
            else if (exception is Exception)
            {
                code = HttpStatusCode.BadRequest;
            }

            context.Response.StatusCode = (int)code;

            if (exception == null)
            {
                return null;
            }

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
