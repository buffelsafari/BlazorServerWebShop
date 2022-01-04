using CosmosDBService.DTO.Product;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                "tool",
                "furniture",
                "food",
                "apperal"
            };


            List<string> pathSeedList = new List<string>()
            {
                "Alpha",
                "Beta",
                "Gamma",
                "Delta",
            };

            List<string> pathSubSeedList1 = new List<string>()
            {
                "VitaminA",
                "VitaminB",
                "VitaminC",                
            };

            List<string> pathSubSeedList2 = new List<string>()
            {
                "Dog",
                "Cat",
                "Rabbit",
            };

            List<string> pathSubSeedList3 = new List<string>()
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

            String descript = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            

            Random rnd = new Random();
                        

            for (int i = 0; i < 100; i++)
            {
                Dictionary<string, Object> dic = new Dictionary<string, Object>();
                string type = $"{typeSeedList[rnd.Next(typeSeedList.Count)]}";

                dic.Add("type", type);
                dic.Add("path", $"{pathSeedList[rnd.Next(pathSeedList.Count)]}/{pathSubSeedList1[rnd.Next(pathSubSeedList1.Count)]}/{pathSubSeedList2[rnd.Next(pathSubSeedList2.Count)]}/{pathSubSeedList3[rnd.Next(pathSubSeedList3.Count)]}/");
                dic.Add("name", $"{nameC1[rnd.Next(nameC1.Count)]}{nameC2[rnd.Next(nameC2.Count)]}{nameC3[rnd.Next(nameC3.Count)]}{nameC4[rnd.Next(nameC4.Count)]}{nameC5[rnd.Next(nameC5.Count)]}");
                dic.Add("price", rnd.Next(1, 9999).ToString());
                dic.Add("price_unit", unitC[rnd.Next(unitC.Count)]);
                dic.Add("image", imageC[rnd.Next(imageC.Count)]);
                dic.Add("product_images", imageC);
                dic.Add("description", descript);

                dic.Add("specifications", new { height="123cm", width="243cm", wifi="v1", screen="lcd" });
                

                switch (type)
                {
                    case "tool":
                        break;
                    case "furniture":
                        break;
                    case "food":
                        break;
                    case "apperal":
                        dic.Add("color_options", new string[] { "red", "green", "blue" });
                        dic.Add("size_options", new string[] { "small", "medium", "large"});
                        break;
                }

                CreateProduct(container, dic).Wait();

            }          

            
                        
        }


        private async Task CreateProduct(Container container, Dictionary<string, Object> parameters)
        {

            ExpandoObject obj = new ExpandoObject();
             
            obj.TryAdd("id", Guid.NewGuid().ToString());
            obj.TryAdd("created", DateTime.UtcNow.ToString());            

            foreach (KeyValuePair<string, object> kvp in parameters)
            {
                obj.TryAdd(kvp.Key, kvp.Value);                
            }

            await container.CreateItemAsync(obj);
        }

        //private async Task CreateProduct(Container container, string type, string path, string name, string price, string priceUnit, string image)
        //{
        //    var annoymous = new 
        //    {
        //        id = Guid.NewGuid().ToString(),
        //        type = type,
        //        path = path,
        //        name = name,
        //        price = price,
        //        price_unit = priceUnit,
        //        image = image,
                
        //    };

        //    await container.CreateItemAsync(annoymous);

        //    //await container.CreateItemAsync<ProductCardDTO>(new ProductCardDTO
        //    //{
        //    //    Id = Guid.NewGuid().ToString(),
        //    //    Type=type,
        //    //    Path = path,
        //    //    Name = name,
        //    //    Price = price,
        //    //    PriceUnit = priceUnit,
        //    //    Image= image,
        //    //});
        //}   

        

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
            FeedIterator<ProductCardDTO> queryResultSetIterator = products.GetItemQueryIterator<ProductCardDTO>(queryDefinition);
            HashSet<string> items = new HashSet<string>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<ProductCardDTO> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (ProductCardDTO product in currentResultSet)
                {
                    items.Add(product.Path);                    
                }
            }
            return items;
        }

        
        public async IAsyncEnumerable<T> GetProducts<T>(string path, string search, ProductSorting sorting, int offset, int limit, [EnumeratorCancellation] CancellationToken cancellationToken=default)
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
            //List<T> items = new List<T>();

            while (queryResultSetIterator.HasMoreResults)
            {
                //FeedResponse<T> currentResultSet = await ;
                foreach (var product in await queryResultSetIterator.ReadNextAsync(cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }                    
                    Task.Delay(300).Wait();  // for test
                                        
                    yield return product;
                }
            }
            
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
