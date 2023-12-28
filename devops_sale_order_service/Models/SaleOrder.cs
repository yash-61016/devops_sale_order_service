namespace devops_sale_order_service.Models
{
    public class SaleOrder
    {
        public int Id { get; set; }
        public required int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerAddress { get; set; }
        public required string CustomerPhone { get; set; }
        public required string CustomerEmail { get; set; }
        public required decimal TotalAmount { get; set; }
        public required bool IsCancelled { get; set; }
        public required List<SaleOrderLine> SaleOrderLines { get; set; }
        public required DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }

}