namespace devops_sale_order_service.Models.Dto.Create
{
    public class SaleOrderLineCreateDto
    {
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalAmount { get; set; }
    }
}