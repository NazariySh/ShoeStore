using System.Text.Json;
using FluentValidation;
using ShoeStore.Application.DTOs;
using ShoeStore.Domain.Exceptions;

namespace ShoeStore.Api.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {ErrorMessage}", exception.Message);

        var statusCode = GetStatusCode(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        string result;

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);

            var response = new ValidationErrorResponseDto(errors);
            result = JsonSerializer.Serialize(response, _options);
        }
        else
        {
            var response = new ErrorResponseDto(exception.Message);
            result = JsonSerializer.Serialize(response, _options);
        }

        return context.Response.WriteAsync(result);
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            ArgumentNullException or ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            ApiException api => api.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}