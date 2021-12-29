using BlazorServerCrud1.Components.PaginatorComponent;
using BlazorServerCrud1.ViewModels;
using CosmosDBData;
using CosmosDBService.DTO;
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
        public string? Type { get; set; }

        [Parameter]
        public string? SubType1 { get; set; }

        [Parameter]
        public string? SubType2 { get; set; }

        [Parameter]
        public string? SubType3 { get; set; }


        [Parameter]
        public IEnumerable<ProductModel> Items { get; set; } = new List<ProductModel>();


        private string? _type;

        


        protected override Task OnInitializedAsync()
        {
            

            //SelectedPage = 0;
            _type = Type;

            

            return base.OnInitializedAsync();

        }


        protected async override Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();

            

            if (Type != _type)  // todo detect search changes
            {
                SelectedPage = 0;
                Debug.WriteLine("Type change detected");
                _type = Type;
                //StateHasChanged();
                
            }

            
            //await GetStuff();

            // todo only db when changes detected
            //TotalCount = await cosmosDB.ValueSQLCount(Type != null ? Type : "", SearchWord != null ? SearchWord : "");            

            string typeCombo = Type != null ? Type + "/" : "";
            typeCombo += SubType1 != null ? SubType1 + "/" : "";
            typeCombo += SubType2 != null ? SubType2 + "/" : "";
            typeCombo += SubType3 != null ? SubType3 + "/" : "";

            Debug.WriteLine("typecombo-------- "+typeCombo);
            
            PaginatorCallback?.Invoke(SelectedPage, await Count(typeCombo));

            var productsDTO = await cosmosDB.GetProducts(typeCombo, SearchWord != null ? SearchWord : "", ProductSorting.name, offset: SelectedPage*ItemsPerPage, limit: ItemsPerPage);

            Items = productsDTO.Select(p => 
                new ProductModel
                {
                    Id=p.Id,
                    Name=p.Name,
                    Type=p.Type,
                    Price=p.Price,
                    PriceUnit=p.PriceUnit,
                    Image=p.Image,
                });

            //Items = new List<ProductModel>();
            //foreach (ProductDTO p in productsDTO)
            //{
            //    Items.Add(new ProductModel
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Type = p.Type,
            //        Price = p.Price,
            //        PriceUnit = p.PriceUnit,
            //    });
            //}


            // await
            


            //StateHasChanged();
            

        }

        

        private Task<int> Count(string type)
        {            
            return cosmosDB.ValueSQLCount(type, SearchWord != null ? SearchWord : "");
        }

        








    }
}
