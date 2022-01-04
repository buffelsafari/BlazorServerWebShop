using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBService.DTO.Product
{
    public class BaseDetailedProductDTO:AbstractBaseProductDTO
    {
        [JsonProperty(PropertyName = "product_images")]
        public IEnumerable<string>? ProductImages { get; set; }
    }
}
