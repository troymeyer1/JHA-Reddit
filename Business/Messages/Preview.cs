using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Preview
{
    [JsonPropertyName("images")]
    public List<Image> Images { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}