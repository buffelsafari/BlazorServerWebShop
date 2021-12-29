using CosmosDBService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBData
{
    public interface ICosmosDBContext
    {
        
        Task<IEnumerable<ProductDTO>> GetProducts(string type, string search, ProductSorting sorting, int offset, int limit);
        Task<int> ValueSQLCount(string type, string search);
        Task<IEnumerable<string>> GetTypes();
        void GenerateTestDB();
    }
}
