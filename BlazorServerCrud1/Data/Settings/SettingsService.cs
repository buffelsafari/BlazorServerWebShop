using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Diagnostics;





namespace BlazorServerCrud1.Data.Settings
{
    public class SettingsService:ISettingsService
    {
        

        public string Name { get;}
        public int DefaultItemsPerPage { get; }

        public IEnumerable<int> PageSizes { get; }

                
        public SettingsService()
        {
            // todo test for exeptions
            string str = System.IO.File.ReadAllText("wwwroot/shop-data/settings.json");
            var settings = JsonConvert.DeserializeObject<SettingsDTO>(str);
                        

            Name = settings.Name != null ? settings.Name : "Default Name";
            DefaultItemsPerPage = settings.DefaultItemsPerPage!=null ? (int)settings.DefaultItemsPerPage:3;

            PageSizes = settings.PageSizes != null ? settings.PageSizes : new List<int>() { 5, 10};
        }

        
    }
}
