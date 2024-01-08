using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// Should probably have a more serious implementation here, but since this is just for the interview, this will do.
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(
            new Response
            {
                TraceId = context.TraceIdentifier,
                IsSuccess = false,
                StatusCode = (int)code,
                Errors =
                [
                    new Error
                    {
                        Code = "Error",
                        Message = exception.Message,
                        AdditionalInfo = string.Empty
                    }
                ]
            }
        );

        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(result);
    }
}
