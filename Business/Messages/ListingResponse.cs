using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;
// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

public class ListingResponse
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}