namespace devops_sale_order_service.Models.Dto
{
    public class SaleOrderLineDto
    {
        public int Id { get; set; }
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalAmount { get; set; }
    }
}