namespace devops_sale_order_service.Models.Dto.Create
{

    public class SaleOrderCreateDto
    {
        public required int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerAddress { get; set; }
        public required string CustomerPhone { get; set; }
        public required string CustomerEmail { get; set; }
        public required List<SaleOrderLineCreateDto> SaleOrderLines { get; set; }
    }
}