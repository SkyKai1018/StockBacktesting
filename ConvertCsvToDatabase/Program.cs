// See https://aka.ms/new-console-template for more information

using System.Numerics;
using Microsoft.EntityFrameworkCore;
using StockBacktesting.Data;
using StockBacktesting.Models;
using EarningsDistribution =StockBacktesting.Models.EarningsDistribution;


Console.WriteLine("Hello, World!");

// 设置 DbContextOptionsBuilder
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer("Server=localhost;Database=Stock;User Id=root;");

string filePath = "/Users/skykai/Desktop/StockBacktesting/ConvertCsvToDatabase/stockDividend_TW.csv";

List<EarningsDistribution> distributions = File.ReadAllLines(filePath)
    .Skip(1) // 跳过标题行
    .Select(line =>
    {
        string[] data = line.Split(',');

        // 尝试解析StockId为整数
        bool isStockIdInteger = int.TryParse(data[1], out int parsedStockId);

        // 尝试解析CashEarningsDistribution为小数
        bool isCashEarningsDistributionDecimal = decimal.TryParse(data[2], out decimal parsedCashEarningsDistribution);

        // 如果StockId和CashEarningsDistribution都能成功解析
        if (isStockIdInteger && isCashEarningsDistributionDecimal)
        {
            return new EarningsDistribution
            {
                Date = DateTime.Parse(data[0]),
                StockId = parsedStockId,
                CashEarningsDistribution = parsedCashEarningsDistribution,
            };
        }
        else
        {
            return null; // 或者其他适当的错误处理
        }
    })
    .Where(distribution => distribution != null) // 确保移除所有未能成功解析的记录
    .ToList();



using (var context = new ApplicationDbContext(optionsBuilder.Options))
{
    //var test = context.TradingDatas.Where(r => r.StockId == 2330).ToList();
    context.EarningsDistributions.AddRange(distributions);
    try
    {
        context.SaveChanges();

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
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