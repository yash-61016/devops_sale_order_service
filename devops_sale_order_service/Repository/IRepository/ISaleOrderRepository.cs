using devops_sale_order_service.Models;

namespace devops_sale_order_service.Repository.IRepository
{
    public interface ISaleOrderRepository
    {
        Task<SaleOrder> GetSaleOrderById(int saleOrderId);
        Task<ICollection<SaleOrder>> GetSaleOrdersByCustomerId(int customerId);
        Task CreateSaleOrder(SaleOrder saleOrder);
        Task UpdateSaleOrder(SaleOrder saleOrder);
        Task SaveAsync();
    }
}