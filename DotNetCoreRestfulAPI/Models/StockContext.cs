using Microsoft.EntityFrameworkCore;

namespace DotNetCoreRestfulAPI.Models
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}
