using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Bj1zkgyzc9ob1
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("e")]
    public string E { get; set; }

    [JsonPropertyName("m")]
    public string M { get; set; }

    [JsonPropertyName("p")]
    public List<P> P { get; set; }

    [JsonPropertyName("s")]
    public S S { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}