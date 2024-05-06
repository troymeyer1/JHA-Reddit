using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class S
{
    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("u")]
    public string U { get; set; }
}