using Microsoft.EntityFrameworkCore;
using StockBacktesting.Models;

namespace StockBacktesting
{
    public class TestConnDBContext : DbContext
    {
        public DbSet<Connection> Connection { get; set; }

        public TestConnDBContext(DbContextOptions<TestConnDBContext> options) : base(options)
        {
        }

    }

}
