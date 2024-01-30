using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockBacktesting.Models;
using System.Globalization;
using System.Text.Json;

namespace StockBacktesting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockDataController : ControllerBase
    {
        private const string CsvFilePath = "output.csv";
        IEnumerable<StockData> csv = ReadDataFromCsv(CsvFilePath);

        #region public

        #region HttpGet
        [HttpGet]
        public IActionResult GetStockData(string stockId)
        {

            var filteredRecords = csv.Where(r => r.StockId == stockId).ToList();
            var json = JsonSerializer.Serialize(filteredRecords, new JsonSerializerOptions { WriteIndented = true });
            return Ok(json);
        }

        [HttpGet("search/{query}")]
        public IActionResult SearchStocks(string query)
        {
            var matchedStocks = csv.Select(record => record.StockId).Distinct().ToList().Where(s => s.StartsWith(query)).ToList();

            // 序列化過濾後的紀錄為JSON
            var json = JsonSerializer.Serialize(matchedStocks, new JsonSerializerOptions { WriteIndented = true });
            return Ok(json);

        }

        [HttpGet("GetCalculateReturn")]
        public IActionResult GetCalculateReturn(string stockId,DateTime startDate, DateTime endDate, int specificDay)
        {

            var filteredRecords = csv.Where(r => r.StockId == stockId && r.Date >= startDate && r.Date <= endDate).ToList();

            var json = JsonSerializer.Serialize(CalculateReturnBySpecificDayOfMonth(filteredRecords, specificDay), new JsonSerializerOptions { WriteIndented = true });
            return Ok(json);

        }

        [HttpGet("GetCalculateReturnDayOfWeek")]
        public IActionResult GetCalculateReturnByDayOfWeek(DateTime startDate, DateTime endDate, DayOfWeek specificDayOfWeek)
        {
            var filteredRecords = csv.Where(r => r.StockId == "2330" && r.Date >= startDate && r.Date <= endDate).ToList();

            var json = JsonSerializer.Serialize(CalculateReturnBySpecificDayOfWeek(filteredRecords, specificDayOfWeek), new JsonSerializerOptions { WriteIndented = true });
            return Ok(json);

        }

        #endregion HttpGet

        #endregion

        #region Private

        #region Method

        private static IEnumerable<StockData> ReadDataFromCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<StockData>().ToList();

        }


        ReturnData CalculateReturnBySpecificDayOfMonth(List<StockData> data, int purchaseDay)
        {
            double totalShares = 0;
            double totalInvestment = 0;
            double investmentPerSpecificDay = 100;


            var filteredData = data;

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
        #endregion Method

        #endregion Private

    }
}


