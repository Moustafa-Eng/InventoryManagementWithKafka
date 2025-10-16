using Microsoft.EntityFrameworkCore;

namespace InventoryConsumer.Models
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            
        }

        public DbSet<InventoryUpdateRequest> InventoryUpdateRequests { get; set; }
    }
}
