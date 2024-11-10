
using CongestionTaxCalculator.Application.Common.Exceptions;
using CongestionTaxCalculator.Domain.City.Exceptions;
using System.Net;
using System.Net.Mime;

namespace CongestionTaxCalculator.API.Middlewares
{
    internal class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exp)
            {
                HttpStatusCode statusCode = HttpStatusCode.BadRequest;
                var logLevel = LogLevel.Information;

                var errorResult = new ErrorResult();
                errorResult.Message = exp.Message;

                switch (exp)
                {
                    case CityNotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        errorResult.Code = ErrorCode.NotFound;
                        break;
                    case MoreThanOneYearException or TaxRuleNotFoundException:
                        statusCode = HttpStatusCode.BadRequest;
                        errorResult.Code = ErrorCode.BadRequest;
                        break;
                    default:
                        logLevel = LogLevel.Error;
                        statusCode = HttpStatusCode.InternalServerError;
                        errorResult.Code = ErrorCode.InternalServerError;
                        errorResult.Message = "Internal server error.";
                        break;
                }

                LogException(exp, context.Request, logLevel);

                var response = context.Response;
                if (!response.HasStarted)
                {
                    response.ContentType = MediaTypeNames.Application.Json;
                    response.StatusCode = (int)statusCode;
                    await response.WriteAsync(errorResult.ToString());
                }
            }
        }

        private void LogException(Exception exception, HttpRequest request, LogLevel logLevel)
        {
            var serializedRequest =
                new
                {
                    method = request.Method,
                    path = request.Path,
                    query = request.QueryString
                };

            _logger.Log(logLevel, "Request finished with {@exception} and {@serializedRequest}", exception, serializedRequest);
        }
    }
}
