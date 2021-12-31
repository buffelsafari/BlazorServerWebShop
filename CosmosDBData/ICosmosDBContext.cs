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
        
        IAsyncEnumerable<T> GetProducts<T>(string path, string search, ProductSorting sorting, int offset, int limit, CancellationToken cancellationToken = default);
        Task<int> ValueSQLCount(string path, string search);
        Task<IEnumerable<string>> GetPaths();
        Task<T?> GetProduct<T>(string id);
        void GenerateTestDB();
    }
}
