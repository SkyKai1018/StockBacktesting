using Microsoft.AspNetCore.Mvc.Rendering;
using StockBacktesting.Models;

public class FilterMethod
{
    public string FilterStrategy { get; set; }
    public IEnumerable<SelectListItem> FilterStrategies { get; set; }
    public int Days { get; set; }
}

public class YourViewModel
{
    public bool StockPrice { get; set; }
    public ComparisonOperators ComparisonOperators { get; set; }
    public int? StockPriceComparison { get; set; }
    public bool RecentDaysRise { get; set; }
    public int? RecentDaysRiseValue { get; set; }
    public bool RecentDaysFall { get; set; }
    public int? RecentDaysFallValue { get; set; }
    public int? RecentDaysChangeMin { get; set; }
    public int? RecentDaysChangeMax { get; set; }
}

public enum FilterStrategy
{
    StockPrice,
    RecentDaysRise,
    RecentDaysFall,
    recentDaysChange,
}

public class FilterResult
{
    public string FilterMethod { get; set; }
    public int MatchCount { get; set; }
}


