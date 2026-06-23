using System.Net;
using System.Text.Json;
using ControleTeste.Exceptions;

namespace ControleTeste.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção desconhecida");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int status = (int)HttpStatusCode.InternalServerError;
        string title = "Ops! Ocorreu um erro por parte do servidor!";

        if (exception is AppException appEx)
        {
            status = appEx.StatusCode;
            title = exception.Message;
        }
        else
        {
            if (_env.IsDevelopment())
            {
                title = exception.Message;
            }
        }

        var problem = new
        {
            title,
            status,
            detail = _env.IsDevelopment() ? exception.ToString() : null,
            instance = context.Request.Path
        };

        var payload = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;
        return context.Response.WriteAsync(payload);
    }
}