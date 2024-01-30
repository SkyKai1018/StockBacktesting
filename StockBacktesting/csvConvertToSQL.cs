using System;
using System.IO;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockBacktesting.Models;
using CsvHelper;
using System.ComponentModel.DataAnnotations;

namespace StockBacktesting
{
    public class TestConnDB : DbContext
    {
        public DbSet<Connection> Connection { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=TestConn;User Id=root;",
                new MySqlServerVersion(new Version(8, 2, 0))); // 確認 MySQL 版本
        }


    }

}
