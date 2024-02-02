using System;
using Microsoft.EntityFrameworkCore;
using StockBacktesting.Models;

namespace StockBacktesting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<TradingData> TradingDatas { get; set; }
        public DbSet<EarningsDistribution> EarningsDistributions { get; set; }
    }
}

