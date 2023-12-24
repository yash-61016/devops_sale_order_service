using devops_sale_order_service.Models;

namespace devops_sale_order_service.Repository.IRepository
{
    public interface ISaleOrderLineRepository
    {
        Task<ICollection<SaleOrderLine>> GetSaleOrderLinesBySaleOrderId(int saleOrderId);
        Task CreateSaleOrderLine(SaleOrderLine saleOrderLine);
        Task UpdateSaleOrderLine(SaleOrderLine saleOrderLine);
        Task SaveAsync();
    }
}