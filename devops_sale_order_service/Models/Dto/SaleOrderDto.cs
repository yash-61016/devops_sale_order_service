namespace devops_sale_order_service.Models.Dto
{
    public class SaleOrderDto
    {
        public int Id { get; set; }
        public required int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerAddress { get; set; }
        public required string CustomerPhone { get; set; }
        public required string CustomerEmail { get; set; }
        public required decimal TotalAmount { get; set; }
        public required List<SaleOrderLineDto> SaleOrderLines { get; set; }
    }
}