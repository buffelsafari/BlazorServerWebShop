using BlazorServerCrud1.Components.PaginatorComponent;
using CosmosDBData;
using CosmosDBService.DTO.Product;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorServerCrud1.Pages
{



    public partial class Products : ComponentBase
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
        public IEnumerable<ProductCardDTO> Items { get; set; } = new List<ProductCardDTO>();


        private string? _path;

        


        protected override Task OnInitializedAsync()
        {
            

            //SelectedPage = 0;
            _path = Path;

            

            return base.OnInitializedAsync();

        }


        protected async override Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();

            

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

            Items = await cosmosDB.GetProducts<ProductCardDTO>(typeCombo, SearchWord != null ? SearchWord : "", ProductSorting.name, offset: SelectedPage*ItemsPerPage, limit: ItemsPerPage);

            
            

        }

        

        private Task<int> Count(string path)
        {            
            return cosmosDB.ValueSQLCount(path, SearchWord != null ? SearchWord : "");
        }

        








    }
}
