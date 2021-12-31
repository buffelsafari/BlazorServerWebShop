using BlazorServerCrud1.Components.PaginatorComponent;
using CosmosDBData;
using CosmosDBService.DTO.Product;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;


namespace BlazorServerCrud1.Pages
{



    public partial class Products : ComponentBase, IDisposable
    {
        //[CascadingParameter]
        //public PaginatorParam? PaginatorCascade { get; set; }

        [CascadingParameter]
        public string? SearchWord { get; set; }



        [CascadingParameter]
        public Action<int, int>? PaginatorCallback { get; set; }

        [CascadingParameter(Name = "ItemsPerPage")]
        public int ItemsPerPage { get; set; }

        [CascadingParameter(Name = "SelectedPage")]
        public int SelectedPage { get; set; }



        



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
        public IAsyncEnumerable<ProductCardDTO> ItemsAsync { get; set; }


        private string? _path;

        private CancellationTokenSource tokenSource;


        protected override Task OnInitializedAsync()
        {
            

            //SelectedPage = 0;
            _path = Path;

            

            return base.OnInitializedAsync();

        }


        protected async override Task OnParametersSetAsync()
        {
            
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();


            if (Path != _path)  // todo detect search changes
            {
                SelectedPage = 0;
                Debug.WriteLine("Type change detected");
                _path = Path;
                //StateHasChanged();
                
            }

            
            //await GetStuff();

            // todo only db when changes detected
            //TotalCount = await cosmosDB.ValueSQLCount(Type != null ? Type : "", SearchWord != null ? SearchWord : "");            

            string typeCombo = Path != null ? Path + "/" : "";
            typeCombo += SubPath1 != null ? SubPath1 + "/" : "";
            typeCombo += SubPath2 != null ? SubPath2 + "/" : "";
            typeCombo += SubPath3 != null ? SubPath3 + "/" : "";

            Debug.WriteLine("typecombo-------- "+typeCombo);
            
            PaginatorCallback?.Invoke(SelectedPage, await Count(typeCombo));

            
            

            await Task.Run(()=>GetCardData(typeCombo, tokenSource.Token), tokenSource.Token);

            
            

            await base.OnParametersSetAsync();
        }

        


        private async Task GetCardData(string typeCombo, CancellationToken token)
        {
            Items.Clear();
            await foreach (var v in cosmosDB.GetProducts<ProductCardDTO>(typeCombo, SearchWord != null ? SearchWord : "", ProductSorting.name, offset: SelectedPage * ItemsPerPage, limit: ItemsPerPage, token))
            {
                if (!token.IsCancellationRequested)
                {
                    await InvokeAsync(() =>
                    {
                        Items?.Add(v);
                        StateHasChanged();
                    });
                }

                
                //StateHasChanged();
                Debug.WriteLine("hello from other Task");
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
    }
}
