using devops_cart_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Models.Dto.Get;

namespace devops_sale_order_service.Service.IService
{
    public interface ISaleOrderService
    {
        Task<APIResponse> CreateSaleOrder(SaleOrderCreateDto saleOrderCreateDto);
        Task<APIResponse> GetSaleOrderByUserId(SaleOrderGetQueryDto saleOrderGetQueryDto);
        Task<APIResponse> UpdateSaleOrder(SaleOrderDto saleOrderUpdateDto);
        Task<APIResponse> DeleteSaleOrder(int saleOrderId);
    }
}