using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class Item
{
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}