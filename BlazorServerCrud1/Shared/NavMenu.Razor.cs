using BlazorServerCrud1.Components.MenuComponents;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Diagnostics;

namespace BlazorServerCrud1.Shared
{
    
    public partial class NavMenu:ComponentBase
    {
        private MenuNode rootMenuNode=new MenuNode("Products", null);
        private IEnumerable<string>? types;

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected async override Task OnInitializedAsync()
        {
            types = await cosmosDB.GetTypes();
                        
            foreach (string type in types)
            {
                rootMenuNode.Add("Products", type);
            }
            
            await base.OnInitializedAsync();
        }
    }
}
