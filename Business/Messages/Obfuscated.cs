using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Obfuscated
{
    [JsonPropertyName("source")]
    public Source Source { get; set; }

    [JsonPropertyName("resolutions")]
    public List<Resolution> Resolutions { get; set; }
}