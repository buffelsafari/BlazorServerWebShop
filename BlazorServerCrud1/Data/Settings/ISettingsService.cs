namespace BlazorServerCrud1.Data.Settings
{
    public interface ISettingsService
    {
        string Name { get; }        
        int DefaultItemsPerPage { get; }

        IEnumerable<int> PageSizes { get; }
    }
}
