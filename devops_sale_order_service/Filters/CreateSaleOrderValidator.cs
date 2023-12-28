using devops_sale_order_service.Models.Dto.Create;
using FluentValidation;

namespace devops_sale_order_service.Filters
{
    public class CreateSaleOrderValidator<T> : AbstractValidator<T> where T : SaleOrderCreateDto
    {
        public CreateSaleOrderValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer Id is required");
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer Name is required");
            RuleFor(x => x.CustomerAddress).NotEmpty().WithMessage("Customer Address is required");
            RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("Customer Phone is required");
            RuleFor(x => x.CustomerEmail).NotEmpty().WithMessage("Customer Email is required");
            RuleFor(x => x.SaleOrderLines).NotEmpty().WithMessage("Sale Order Lines is required");
            RuleForEach(x => x.SaleOrderLines).SetValidator(new CreateSaleOrderLineValidator());
        }
    }
}