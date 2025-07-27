using System.Text.Json.Serialization;

namespace WebApi.Models.Dto
{
    public class ApiResponseDto<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        public bool? IsSuccess { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
