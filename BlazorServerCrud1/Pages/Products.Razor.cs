using BlazorServerCrud1.Components.PaginatorComponent;
using CosmosDBData;
using CosmosDBService.DTO.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;



namespace BlazorServerCrud1.Pages
{



    public partial class Products : ComponentBase, IDisposable
    {
        
        [CascadingParameter]
        public string? SearchWord { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "size")]
        public int ItemsPerPage { get; set; }        
        
        [Parameter]
        [SupplyParameterFromQuery(Name = "page")]
        public int SelectedPage { get; set; }        
        
        private int totalNumberOfItems;
        //private string totalPath="";
        private string? oldPath = "";


        

        [Parameter]
        public string? Path { get; set; } = "";

        



        [Parameter]
        public List<ProductCardDTO> Items { get; set; } =new List<ProductCardDTO>();

        [Parameter]
        public IAsyncEnumerable<ProductCardDTO>? ItemsAsync { get; set; }


        

        private CancellationTokenSource? tokenSource;


        protected async override Task OnInitializedAsync()
        {
            // todo unsubscribe?, memory leakage?
            navManager.LocationChanged -= OnLocChange;
            navManager.LocationChanged += OnLocChange;

            var result = await localStorage.GetAsync<int>("ItemsPerPage");
            ItemsPerPage = result.Success ? result.Value : settings.DefaultItemsPerPage;

            await base.OnInitializedAsync();

        }

        private void OnLocChange(object? sender, LocationChangedEventArgs e)
        {
            string str = navManager.ToBaseRelativePath(navManager.Uri);
            Debug.WriteLine("location changed:"+ str);
            
        }

        protected async override Task OnParametersSetAsync()
        {
            

            if (ItemsPerPage < 1)
            { 
                ItemsPerPage = 12;
            }


            Debug.WriteLine("on parameter set");

                       
            

            if (Path != oldPath)
            {
                SelectedPage = 0;
                totalNumberOfItems = await Count(Path);
            }

            oldPath = Path;


            


            

            await GetCardData();



            


            await base.OnParametersSetAsync();
        }


        private async Task GetCardData()
        {
            

            
            

            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            await Task.Run(() => GetCardData(Path, tokenSource.Token), tokenSource.Token);

        }



        private async Task GetCardData(string path, CancellationToken token)
        {            

            Items.Clear();
            await foreach (var v in cosmosDB.GetProducts<ProductCardDTO>(path, SearchWord != null ? SearchWord : "", ProductSorting.name, offset: SelectedPage * ItemsPerPage, limit: ItemsPerPage, token))
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
            navManager.LocationChanged -= OnLocChange;

            tokenSource?.Cancel();
            tokenSource?.Dispose();
            
        }

        public async Task OnSelectPage(int i)
        {            
            SelectedPage = i;
            navManager.NavigateTo("/Products/"+Path + $"?page={SelectedPage}&size={ItemsPerPage}");

            await GetCardData();
            
        }

        public async Task OnSelectPageSize(int i)
        {
            await localStorage.SetAsync("ItemsPerPage", i);

            ItemsPerPage = i;
            SelectedPage = 0;
            navManager.NavigateTo("/Products/" + Path + $"?page={SelectedPage}&size={ItemsPerPage}");

            await GetCardData();

        }

    }
}
