using System.Text.Json.Serialization;

namespace IdentityApi.Models.Dtos
{
    public class AuthResponseDto<T>
    {
        [JsonPropertyName("data")]
        public T? TResponse { get; set; }
        public bool? IsSuccess { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
