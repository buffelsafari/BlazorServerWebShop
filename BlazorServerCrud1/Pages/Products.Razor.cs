using BlazorServerCrud1.Components.PaginatorComponent;
using CosmosDBData;
using CosmosDBService.DTO.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BlazorServerCrud1.Pages
{



    public partial class Products : ComponentBase, IDisposable
    {
        
        [CascadingParameter]
        public string? SearchWord { get; set; }

        
        private int itemsPerPage=10;        
        private int selectedPage = 0;        
        private int totalNumberOfItems;
        private string totalPath="";


        [Parameter]
        [SupplyParameterFromQuery(Name ="id")]
        public int? Id { get; set; } = null;
        

        [Parameter]
        public string? Path { get; set; }

        [Parameter]
        public string? SubPath1 { get; set; }

        [Parameter]
        public string? SubPath2 { get; set; }

        [Parameter]
        public string? SubPath3 { get; set; }




        [Parameter]
        public List<ProductCardDTO> Items { get; set; } =new List<ProductCardDTO>();

        [Parameter]
        public IAsyncEnumerable<ProductCardDTO>? ItemsAsync { get; set; }


        

        private CancellationTokenSource? tokenSource;


        protected override Task OnInitializedAsync()
        {
            

            //SelectedPage = 0;
            //_path = Path;



            return base.OnInitializedAsync();

        }


        protected async override Task OnParametersSetAsync()
        {
            


            Debug.WriteLine("on parameter set");

            totalPath = Path != null ? Path + "/" : "";
            totalPath += SubPath1 != null ? SubPath1 + "/" : "";
            totalPath += SubPath2 != null ? SubPath2 + "/" : "";
            totalPath += SubPath3 != null ? SubPath3 + "/" : "";


            Debug.WriteLine(navManager.BaseUri.ToString());

            IReadOnlyDictionary<string, object?> dict = new Dictionary<string, object?>();
            string str = navManager.GetUriWithQueryParameters("/products/"+totalPath, dict);
            navManager.NavigateTo(str);








            totalNumberOfItems = await Count(totalPath);

            await GetCardData();
            

            await base.OnParametersSetAsync();
        }


        private async Task GetCardData()
        {
            

            
            

            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            await Task.Run(() => GetCardData(totalPath, tokenSource.Token), tokenSource.Token);

        }



        private async Task GetCardData(string path, CancellationToken token)
        {            

            Items.Clear();
            await foreach (var v in cosmosDB.GetProducts<ProductCardDTO>(path, SearchWord != null ? SearchWord : "", ProductSorting.name, offset: selectedPage * itemsPerPage, limit: itemsPerPage, token))
            {
                if (!token.IsCancellationRequested)
                {
                    await InvokeAsync(() =>
                    {
                        Items?.Add(v);
                        StateHasChanged();
                    });
                }
            }

        }
        

        private Task<int> Count(string path)
        {            
            return cosmosDB.ValueSQLCount(path, SearchWord != null ? SearchWord : "");
        }

        

        public void Dispose()
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            
        }

        public async Task OnSelectPage(int i)
        {            
            selectedPage = i;            
            
            
            await GetCardData();
            
        }
    }
}
