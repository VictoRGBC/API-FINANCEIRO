using System.Net;
using System.Text.Json;
using FinancialManager.Domain.Exceptions;

namespace FinancialManager.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        object response;

        switch (exception)
        {
            case NotFoundException notFound:
                statusCode = HttpStatusCode.NotFound;
                response = new { error = notFound.Message, type = "NotFound" };
                break;

            case ValidationException validation:
                statusCode = HttpStatusCode.BadRequest;
                response = new { error = validation.Message, errors = validation.Errors, type = "Validation" };
                break;

            case InsufficientBalanceException insufficient:
                statusCode = HttpStatusCode.BadRequest;
                response = new
                {
                    error = insufficient.Message,
                    currentBalance = insufficient.CurrentBalance,
                    requiredAmount = insufficient.RequiredAmount,
                    type = "InsufficientBalance"
                };
                break;

            case BusinessRuleException businessRule:
                statusCode = HttpStatusCode.BadRequest;
                response = new { error = businessRule.Message, type = "BusinessRule" };
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                response = new { error = "Acesso não autorizado.", type = "Unauthorized" };
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                response = new { error = "Ocorreu um erro interno no servidor.", type = "InternalError" };
                break;
        }

        var finalResponse = new
        {
            error = GetErrorMessage(response),
            statusCode = (int)statusCode,
            timestamp = DateTime.UtcNow,
            type = GetErrorType(response)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(finalResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(jsonResponse);
    }

    private static string GetErrorMessage(object response)
    {
        var type = response.GetType();
        var prop = type.GetProperty("error");
        return prop?.GetValue(response)?.ToString() ?? "Erro desconhecido";
    }

    private static string GetErrorType(object response)
    {
        var type = response.GetType();
        var prop = type.GetProperty("type");
        return prop?.GetValue(response)?.ToString() ?? "Unknown";
    }
}
