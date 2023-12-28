namespace devops_sale_order_service.Models.Dto.Get
{
    public class SaleOrderGetQueryDto
    {
        public int UserId { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}