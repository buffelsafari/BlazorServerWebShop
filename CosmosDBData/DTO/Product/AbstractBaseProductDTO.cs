using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CosmosDBService.DTO.Product
{
    public abstract class AbstractBaseProductDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }
        
        [JsonProperty(PropertyName = "path")]
        public string? Path { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string? Price { get; set; }

        [JsonProperty(PropertyName = "price_unit")]
        public string? PriceUnit { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string? Image { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string? Description { get; set; }

        [JsonProperty(PropertyName = "specifications")]
        public JObject? Specifications { get; set; }

        


    }
}
