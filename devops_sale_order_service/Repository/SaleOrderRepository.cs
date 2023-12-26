using devops_sale_order_service.Data;
using devops_sale_order_service.Models;
using devops_sale_order_service.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace devops_sale_order_service.Repository
{
    public class SaleOrderRepository : ISaleOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public SaleOrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SaleOrder> GetSaleOrderById(int saleOrderId)
        {
            return await _db.SaleOrders.FirstOrDefaultAsync(x => x.Id == saleOrderId) ?? throw new Exception("Sale order not found");
        }

        public async Task<ICollection<SaleOrder>> GetSaleOrdersByCustomerId(int customerId)
        {
            return await _db.SaleOrders.Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task CreateSaleOrder(SaleOrder saleOrder)
        {
            saleOrder.CreatedDate = DateTimeOffset.Now;
            saleOrder.UpdatedDate = DateTimeOffset.Now;
            await _db.SaleOrders.AddAsync(saleOrder);
            await SaveAsync();
        }

        public async Task UpdateSaleOrder(SaleOrder saleOrder)
        {
            saleOrder.UpdatedDate = DateTimeOffset.Now;
            _db.SaleOrders.Update(saleOrder);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}