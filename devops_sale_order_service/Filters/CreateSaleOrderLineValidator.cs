using devops_sale_order_service.Models.Dto.Create;
using FluentValidation;

namespace devops_sale_order_service.Filters
{
    public class CreateSaleOrderLineValidator : AbstractValidator<SaleOrderLineCreateDto>
    {
        public CreateSaleOrderLineValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.UnitPrice).NotEmpty();
        }
    }
}