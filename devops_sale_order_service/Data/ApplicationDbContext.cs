using devops_sale_order_service.Models;
using Microsoft.EntityFrameworkCore;

namespace devops_sale_order_service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderLine> SaleOrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleOrder>().HasKey(s => s.Id);
            modelBuilder.Entity<SaleOrderLine>().HasKey(s => s.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}