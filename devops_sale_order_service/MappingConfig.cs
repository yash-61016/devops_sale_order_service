using AutoMapper;
using devops_sale_order_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;

namespace devops_sale_order_service
{
    public class MappingConfig : Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<SaleOrder, SaleOrderDto>();
                config.CreateMap<SaleOrderLine, SaleOrderLineDto>();
                config.CreateMap<SaleOrderCreateDto, SaleOrder>();
                config.CreateMap<SaleOrderLineCreateDto, SaleOrderLine>();
            });
            return mappingConfig;
        }
    }
}