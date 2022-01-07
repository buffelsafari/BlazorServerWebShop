using Newtonsoft.Json;

namespace BlazorServerCrud1.Data.Settings
{
    public record SettingsDTO
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "default_items_per_page")]
        public int? DefaultItemsPerPage { get; set; }

        [JsonProperty(PropertyName = "page_sizes")]
        public IEnumerable<int>? PageSizes { get; set; }
    }
}
