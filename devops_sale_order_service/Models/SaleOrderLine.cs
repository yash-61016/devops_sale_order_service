namespace devops_sale_order_service.Models
{
    public class SaleOrderLine
    {
        public int Id { get; set; }
        public required int SaleOrderId { get; set; }
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalAmount { get; set; }
        public required DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}