using System;
namespace StockBacktesting.Models
{
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

