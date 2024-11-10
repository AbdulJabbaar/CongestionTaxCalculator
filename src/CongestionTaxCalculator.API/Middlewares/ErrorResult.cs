using System.Text.Json.Serialization;
using System.Text.Json;

namespace CongestionTaxCalculator.API.Middlewares
{
    public enum ErrorCode
    {
        Error = 0, // Default Value
        NotFound,
        Conflict,
        BadRequest,
        InvalidInput,
        InternalServerError
    }

    public class ErrorResult
    {
        public ErrorCode Code { get; set; } = default;
        public string Message { get; set; } = default!;
        public DateTime Timestamp { get; }

        public ErrorResult()
        {
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Serialize(this, jsonSerializerOptions);
        }
    }
}
