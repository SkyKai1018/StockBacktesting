using StockBacktesting.Models;

public class FilterViewModel
{
    public List<Filter> Filters { get; set; }
    public List<Stock> GroupedRecords { get; set; }
    public TradingData TradingData { get; set; }
    public double? Seconds { get; set; }
    // Add other properties as needed
}