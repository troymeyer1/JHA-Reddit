using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Child
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}