using devops_sale_order_service.Data;
using devops_sale_order_service.Models;
using devops_sale_order_service.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace devops_sale_order_service.Repository
{
    public class SaleOrderLineRepository : ISaleOrderLineRepository
    {
        private readonly ApplicationDbContext _db;

        public SaleOrderLineRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateSaleOrderLine(SaleOrderLine saleOrderLine)
        {
            await _db.SaleOrderLines.AddAsync(saleOrderLine);
            await SaveAsync();
        }

        public async Task<ICollection<SaleOrderLine>> GetSaleOrderLinesBySaleOrderId(int saleOrderId)
        {
            return await _db.SaleOrderLines.Where(x => x.SaleOrderId == saleOrderId).ToListAsync();
        }

        public async Task UpdateSaleOrderLine(SaleOrderLine saleOrderLine)
        {
            _db.SaleOrderLines.Update(saleOrderLine);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}