namespace BlazorServerCrud1.Data.Settings
{
    public interface ISettingsService
    {
        string Name { get; }
        int ItemsPerPage { get; set; }
        int DefaultItemsPerPage { get; }
    }
}
