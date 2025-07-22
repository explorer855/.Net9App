using System.Text.Json.Serialization;

namespace AuthApi.Models.Dtos
{
    public class AuthResponseDto
    {
        [JsonPropertyName("data")]
        public object? TResponse { get; set; }
        public bool? Success { get; set; } = true;
    }
}
