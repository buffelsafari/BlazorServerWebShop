using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBService.DTO.Product
{
    public class ApperalProductDTO:AbstractBaseProductDTO
    {
        [JsonProperty(PropertyName = "color_options")]
        public IEnumerable<string>? ColorOptions { get; set; }
    }
}
