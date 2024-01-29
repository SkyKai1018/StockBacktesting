using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static StockBacktesting.Controllers.StockDataController;

namespace StockBacktesting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockDataController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStockData(string stockId)
        {
            List<StockData> filteredRecords = new List<StockData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader("output.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                filteredRecords = csv.GetRecords<StockData>()
                    .Where(r => r.StockId == stockId)
                    .ToList();

                // 序列化過濾後的紀錄為JSON
                var json = JsonSerializer.Serialize(filteredRecords, new JsonSerializerOptions { WriteIndented = true });
                return Ok(json);
            }
        }

        [HttpGet("search/{query}")]
        public IActionResult SearchStocks(string query)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader("output.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<StockData>();
                var uniqueStockIds = records.Select(record => record.StockId).Distinct().ToList();
                var matchedStocks = uniqueStockIds.Where(s => s.StartsWith(query)).ToList();
                
                // 序列化過濾後的紀錄為JSON
                var json = JsonSerializer.Serialize(matchedStocks, new JsonSerializerOptions { WriteIndented = true });
                return Ok(json);
            }
        }

        [HttpGet("GetCalculateReturn")]
        public IActionResult GetCalculateReturn(DateTime startDate, DateTime endDate, int specificDay)
        {
            List<StockData> filteredRecords = new List<StockData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader("output.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                filteredRecords = csv.GetRecords<StockData>()
                    .Where(r => r.StockId == "2330" && r.Date >= startDate && r.Date <= endDate)
                    .ToList();
                var json = JsonSerializer.Serialize(CalculateReturnBySpecificDayOfMonth(filteredRecords, specificDay), new JsonSerializerOptions { WriteIndented = true });
                return Ok(json);
            }

        }

        [HttpGet("GetCalculateReturnDayOfWeek")]
        public IActionResult GetCalculateReturnByDayOfWeek(DateTime startDate, DateTime endDate, DayOfWeek specificDayOfWeek)
        {
            List<StockData> filteredRecords = new List<StockData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader("output.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                filteredRecords = csv.GetRecords<StockData>()
                    .Where(r => r.StockId == "2330" && r.Date >= startDate && r.Date <= endDate)
                    .ToList();
                var json = JsonSerializer.Serialize(CalculateReturnBySpecificDayOfWeek(filteredRecords, specificDayOfWeek), new JsonSerializerOptions { WriteIndented = true });
                return Ok(json);
            }

        }

        public class ReturnData
        {
            public ReturnData(double totalInvestment, double finalMarketValue, double totalReturn, double returnRate,int days)
            {
                TotalInvestment = totalInvestment;
                FinalMarketValue = finalMarketValue;
                TotalReturn = totalReturn;
                ReturnRate = returnRate;
                Days = days;
            }

            public double TotalInvestment { get; set; }
            public double FinalMarketValue { get; set; }
            public double TotalReturn { get; set; }
            public double ReturnRate { get; set; }
            public double Days { get; set; }


        }

        ReturnData CalculateReturnBySpecificDayOfMonth(List<StockData> data, int purchaseDay)
        {
            double totalShares = 0;
            double totalInvestment = 0;
            double investmentPerSpecificDay = 100;

            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2024, 1, 2);

            var filteredData = data.Where(day => day.Date >= startDate && day.Date <= endDate)
                .OrderBy(day => day.Date)
                .ToList();

            int lastMonth = -1;
            bool purchasedThisMonth = false;
            int days = 0;

            foreach (var day in filteredData)
            {
                if (day.Date.Month != lastMonth)
                {
                    // 重置標記，新的一個月
                    purchasedThisMonth = false;
                    lastMonth = day.Date.Month;
                }

                if (!purchasedThisMonth && (day.Date.Day >= purchaseDay || day.Date.Month != lastMonth))
                {
                    // 購買操作
                    double sharesBought = investmentPerSpecificDay / day.OpenPrice;
                    totalShares += sharesBought;
                    totalInvestment += investmentPerSpecificDay;
                    purchasedThisMonth = true; // 標記當月已購買
                    days++;
                }
            }

            double finalMarketValue = totalShares * filteredData.Last().ClosePrice;
            double totalReturn = finalMarketValue - totalInvestment;
            double returnRate = (totalReturn / totalInvestment) * 100;

            return new ReturnData(totalInvestment, finalMarketValue, totalReturn, returnRate, days);
        }


        ReturnData CalculateReturnByDaily(List<StockData> data)
        {
            double totalShares = 0;
            double totalInvestment = 0;
            double investmentPerDay = 100;

            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2024, 1, 2);

            var filteredData = data.Where(day => day.Date >= startDate && day.Date <= endDate).ToList();

            foreach (var day in filteredData)
            {
                double sharesBought = investmentPerDay / day.OpenPrice;
                totalShares += sharesBought;
                totalInvestment += investmentPerDay;
            }

            double finalMarketValue = totalShares * data.Last().ClosePrice;
            double totalReturn = finalMarketValue - totalInvestment;
            double returnRate = (totalReturn / totalInvestment) * 100;

            return new ReturnData(totalInvestment, finalMarketValue, totalReturn, returnRate, filteredData.Count);
        }

        ReturnData CalculateReturnBySpecificDayOfWeek(List<StockData> data, DayOfWeek specificDayOfWeek)
        {
            double totalShares = 0;
            double totalInvestment = 0;
            double investmentPerDay = 100;

            // 篩選出特定星期的數據
            var filteredData = data.Where(day => day.Date.DayOfWeek == specificDayOfWeek).ToList();

            foreach (var day in filteredData)
            {
                double sharesBought = investmentPerDay / day.OpenPrice;
                totalShares += sharesBought;
                totalInvestment += investmentPerDay;
            }

            double finalMarketValue = totalShares * filteredData.LastOrDefault()?.ClosePrice ?? 0;
            double totalReturn = finalMarketValue - totalInvestment;
            double returnRate = (totalInvestment > 0) ? (totalReturn / totalInvestment) * 100 : 0;

            return new ReturnData(totalInvestment, finalMarketValue, totalReturn, returnRate, filteredData.Count);
        }

    }

    public class StockData
    {
        public DateTime Date { get; set; }
        public string StockId { get; set; }
        public int TradingVolume { get; set; }
        public double TradingMoney { get; set; }
        public double OpenPrice { get; set; }
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public double ClosePrice { get; set; }
        public double Spread { get; set; }
        public int TradingTurnover { get; set; }
    }

}


