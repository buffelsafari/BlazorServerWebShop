using CosmosDBService.DTO;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBData
{
    public enum ProductSorting
    { 
        none=0,
        low_price=1,
        high_price=2,
        name=3,
        
    }

    


    public class CosmosDBContext:ICosmosDBContext
    {
        

        private const string databaseId = "PracticeWebShop1";
        private const string productsContainerId = "Products";

        private CosmosClient cosmosClient;
        private Database database;
        private Container products;

        public CosmosDBContext()
        {
            // todo hide in secrets
            cosmosClient = new CosmosClient("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            database = cosmosClient.GetDatabase(databaseId);
            products = database.GetContainer(productsContainerId);
        }

        public void GenerateTestDB()
        {            
            Database db=cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;

            ContainerProperties containerProperties = new ContainerProperties()
            {
                Id = productsContainerId,
                PartitionKeyPath = "/path/name",
                IndexingPolicy = new IndexingPolicy()
                {
                    Automatic = false,
                    IndexingMode = IndexingMode.Lazy,
                }
            };

            
            Container container=db.CreateContainerIfNotExistsAsync(containerProperties).Result;
            container.DeleteContainerAsync().Wait();
            container = db.CreateContainerIfNotExistsAsync(containerProperties).Result;

            
            List<string> typeSeedList = new List<string>()
            {
                "Alpha",                
                "Beta",
                "Gamma",
                "Delta",
            };

            List<string> typeSubSeedList1 = new List<string>()
            {
                "VitaminA",
                "VitaminB",
                "VitaminC",                
            };

            List<string> typeSubSeedList2 = new List<string>()
            {
                "Dog",
                "Cat",
                "Rabbit",
            };

            List<string> typeSubSeedList3 = new List<string>()
            {
                "Rock",
                "Paper",
                "Scissor",
            };

            List<string> nameC1 = new List<string>()  // prefix
            {                
                "ultra ",
                "extreme ",
                "extra ",
                "premium ",
                "fanzy ",
                "salty ",
                "smooth ",
                "micro ",                
                "",
            };

            List<string> nameC2 = new List<string>()  // prefix
            {
                "small ",
                "large ",
                "girthy ",
                "slim ",
                "fast ",
                "slow ",
                "red ",
                "green ",
                "blue ",
                "yellow ",
                "purple ",
                "black ",
                "white ",
                "semi ",
                "fully ",
                "",
            };

            List<string> nameC3 = new List<string>()  // prefix
            {
                "metalic ",
                "rubber ",
                "ecological ",
                "electric ",
                "manual ",
                "automatic ",
                "recycled ",
                "virgin ",
                "plastic ",
                "",
            };

            List<string> nameC4 = new List<string>()  // the main word
            {
                "duck",
                "glove",
                "nipple",
                "duck",
                "table",
                "computer",
                "gun",
                "knife",
                "cheese",                
            };

            List<string> nameC5 = new List<string>()  // postfix
            {
                " twister",
                " bender",
                " cutter",
                " bag",
                " box",
                " burner",
                " calibrator",
                " cleaner",
                " painter",
                "",
            };

            List<string> unitC = new List<string>()  // todo rethink
            {
                "kr",
                "kr/kg",
                "kr/pack",
                "kr/liter",                
            };

            List<string> imageC = new List<string>()
            {
                "images/ananas.jpg",
                "images/atlas.jpg",
                "images/buffel.jpg",
                "images/cyckel.jpg",
                "images/dator.jpg",
                "images/eggs.jpg",
                "images/el.jpg",
                "images/humidor.jpg",
                "images/ost.jpg",
                "images/queenph.png",
                "images/skinka.jpg",
                "images/skinka2.jpg",
                "images/sphere.png",
                "images/traktor.jpg",
                "images/trebuchet.jpg",                
            };


            Random rnd = new Random();
                        

            for (int i = 0; i < 100; i++)
            {
                CreateProduct(
                    container, 
                    $"{typeSeedList[rnd.Next(typeSeedList.Count)]}/{typeSubSeedList1[rnd.Next(typeSubSeedList1.Count)]}/{typeSubSeedList2[rnd.Next(typeSubSeedList2.Count)]}/{typeSubSeedList3[rnd.Next(typeSubSeedList3.Count)]}/",
                    $"{nameC1[rnd.Next(nameC1.Count)]}{nameC2[rnd.Next(nameC2.Count)]}{nameC3[rnd.Next(nameC3.Count)]}{nameC4[rnd.Next(nameC4.Count)]}{nameC5[rnd.Next(nameC5.Count)]}", 
                    rnd.Next(1, 9999).ToString(),
                    unitC[rnd.Next(unitC.Count)],
                    imageC[rnd.Next(imageC.Count)]).Wait();
            }          

            
                        
        }

        


        private async Task CreateProduct(Container container, string path, string name, string price, string priceUnit, string image)
        {
            
            await container.CreateItemAsync<ProductDTO>(new ProductDTO
            {
                Id = Guid.NewGuid().ToString(),
                Path = path,
                Name = name,
                Price = price,
                PriceUnit = priceUnit,
                Image= image,
            });
        }   

        

        public async Task<int> ValueSQLCount(string path, string search)
        {
            Debug.WriteLine("Database count...");
            QueryDefinition queryDefinition = new QueryDefinition($"SELECT VALUE COUNT(1) FROM c WHERE STARTSWITH(c.path, '{path}') AND c.name LIKE '{search}%'");
            FeedIterator<int> queryResultSetIterator = products.GetItemQueryIterator<int>(queryDefinition);
            List<int> items = new List<int>();
            
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<int> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                return currentResultSet.FirstOrDefault();                
            }
            return 0;

        }

        public async Task<IEnumerable<string>> GetPaths()
        {
            Debug.WriteLine("getting types from database...");
            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c");
            FeedIterator<ProductDTO> queryResultSetIterator = products.GetItemQueryIterator<ProductDTO>(queryDefinition);
            HashSet<string> items = new HashSet<string>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<ProductDTO> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (ProductDTO product in currentResultSet)
                {
                    items.Add(product.Path);                    
                }
            }
            return items;
        }


        public async Task<IEnumerable<T>> GetProducts<T>(string path, string search, ProductSorting sorting, int offset, int limit)
        {
            Debug.WriteLine("Getting products from database...");  // todo watch for extra calls to db
            string order = "";
            switch (sorting)
            {
                case ProductSorting.none:
                    order = "";
                    break;
                case ProductSorting.low_price:
                    order = "ORDER BY c.price DESC";
                    break;
                case ProductSorting.high_price:
                    order = "ORDER BY c.price ASC";
                    break;
                case ProductSorting.name:
                    order = "ORDER BY c.name ASC";
                    break;
            }            
                                    
            // todo maybe change search from like to contains
            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE STARTSWITH(c.path, '{path}') AND c.name LIKE '{search}%' {order} OFFSET {offset} LIMIT {limit}");
            FeedIterator<T> queryResultSetIterator = products.GetItemQueryIterator<T>(queryDefinition);
            List<T> items = new List<T>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (T product in currentResultSet)
                {
                    items.Add(product);                    
                }
            }
            return items;
        }

        public async Task<T?> GetProduct<T>(string id)
        {
            Debug.WriteLine("getting single item from database..");

            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{id}'");
            FeedIterator<T> queryResultSetIterator = products.GetItemQueryIterator<T>(queryDefinition);

            
            if (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();                
                return currentResultSet.FirstOrDefault();                
            }

            return default(T);
            
        }




    }
}
