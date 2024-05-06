using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Source
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}