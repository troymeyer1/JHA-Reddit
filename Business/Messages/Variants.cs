using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Variants
{
    [JsonPropertyName("obfuscated")]
    public Obfuscated Obfuscated { get; set; }

    [JsonPropertyName("nsfw")]
    public Nsfw Nsfw { get; set; }
}