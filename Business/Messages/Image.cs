using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Image
{
    [JsonPropertyName("source")]
    public Source Source { get; set; }

    [JsonPropertyName("resolutions")]
    public List<Resolution> Resolutions { get; set; }

    [JsonPropertyName("variants")]
    public Variants Variants { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}