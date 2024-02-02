using System.Text.Json.Serialization;

public class Data
{
    [JsonPropertyName("industry_category")]
    public string IndustryCategory { get; set; }

    [JsonPropertyName("stock_id")]
    public string StockId { get; set; }

    [JsonPropertyName("stock_name")]
    public string StockName { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }
}

public class ResponseData
{
    [JsonPropertyName("msg")]
    public string Message { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("data")]
    public List<Data> Data { get; set; }
}

//string filePath = @"stockInfo.json";
//string jsonString;

//using (var reader = new StreamReader(filePath))
//{
//    jsonString = reader.ReadToEnd();
//}

//var InfoData = JsonSerializer.Deserialize<ResponseData>(jsonString).Data;

//foreach (var item in InfoData)
//{
//    int parsedStockId;
//    bool isInteger = int.TryParse(item.StockId, out parsedStockId);

//    if (isInteger)
//    {
//        // item.StockId 是有效的整數，繼續檢查資料庫
//        var existingStock = _context.Stocks.FirstOrDefault(s => s.StockId == parsedStockId);
//        if (existingStock == null)
//        {
//            // 如果不存在，則創建並添加新股票
//            var stock = new Stock { StockId = parsedStockId, StockName = item.StockName }; // 替換 "123" 為你需要的股票名稱
//            _context.Stocks.Add(stock);
//            _context.SaveChanges();
//        }
//        else
//        {
//            // 如果已存在，你可以選擇記錄該信息，或者進行其他操作
//            // 例如：Console.WriteLine("Stock with StockId: {0} already exists.", parsedStockId);
//        }
//    }
//}

//var groupedRecords = csv
//    .GroupBy(r => r.StockId)
//    .Select(group => new { StockId = group.Key, Records = group.ToList() })
//    .ToList();

//foreach (var group in groupedRecords)
//{

//    foreach (var item in group.Records)
//    {
//        {
//            int id = 0;
//            try
//            {
//                id = int.Parse(item.StockId);
//                var tradingData = new TradingData
//                {
//                    Date = item.Date,
//                    MaxPrice = (decimal)item.MaxPrice,
//                    MinPrice = (decimal)item.MinPrice,
//                    StockId = int.Parse(item.StockId),
//                    OpenPrice = (decimal)item.OpenPrice,
//                    Spread = (decimal)item.Spread,
//                    TradingMoney = (long)item.TradingMoney,
//                    TradingTurnover = (int)item.TradingTurnover,
//                    TradingVolume = (long)item.TradingVolume,
//                    ClosePrice = (decimal)item.ClosePrice,
//                };
//                _context.TradingDatas.Add(tradingData);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//    }
//    try
//    {
//        _context.SaveChanges();
//    }
//    catch (DbUpdateException ex)
//    {
//        // 输出到控制台或日志
//        Console.WriteLine("An error occurred while saving the entity changes:");
//        Console.WriteLine(ex.Message);

//        if (ex.InnerException != null)
//        {
//            Console.WriteLine("Inner exception details:");
//            Console.WriteLine(ex.InnerException.Message);
//        }
//    }

//}