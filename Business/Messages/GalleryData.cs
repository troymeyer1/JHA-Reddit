using System.Text.Json.Serialization;

namespace Jha.Reddit.Business.Messages;

public class GalleryData
{
    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }
}