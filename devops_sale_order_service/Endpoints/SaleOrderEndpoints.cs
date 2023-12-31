using devops_cart_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Models.Dto.Get;
using devops_sale_order_service.Service.IService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace devops_sale_order_service.Endpoints
{
    public static class SaleOrderEndpoints
    {
        public static void ConfigureSaleOrderEndpoints(this WebApplication app)
        {
            app.MapPost("/api/saleorder/create", CreateSaleOrder)
            .WithName("CreateSaleOrder")
            .Accepts<SaleOrderCreateDto>("application/json")
            .Produces<APIResponse>(201)
            .Produces(400);

            app.MapGet("/api/saleorder", GetSaleOrderByUserId)
            .WithName("GetSaleOrderByUserId")
            .Accepts<SaleOrderGetQueryDto>("application/json")
            .Produces<APIResponse>(200)
            .Produces(400);

            app.MapPut("/api/saleorder/update", UpdateSaleOrder)
            .WithName("UpdateSaleOrder")
            .Accepts<SaleOrderDto>("application/json")
            .Produces<APIResponse>(200)
            .Produces(400);

            app.MapDelete("/api/saleorder/delete-{{saleOrderId}}", DeleteSaleOrder)
            .WithName("DeleteSaleOrder")
            .Accepts<int>("application/json")
            .Produces<APIResponse>(200)
            .Produces(400);

        }

        private async static Task<IResult> CreateSaleOrder(
            ISaleOrderService _saleOrderService,
            [FromBody] SaleOrderCreateDto saleOrderCreateDto
        )
        {
            var result = await _saleOrderService.CreateSaleOrder(saleOrderCreateDto);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }

        private async static Task<IResult> GetSaleOrderByUserId(
            ISaleOrderService _saleOrderService,
            [FromBody] SaleOrderGetQueryDto queryDto
        )
        {
            var result = await _saleOrderService.GetSaleOrderByUserId(queryDto);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }


        private async static Task<IResult> UpdateSaleOrder(
            ISaleOrderService _saleOrderService,
            [FromBody] SaleOrderDto saleOrderUpdateDto
        )
        {
            var result = await _saleOrderService.UpdateSaleOrder(saleOrderUpdateDto);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }

        private async static Task<IResult> DeleteSaleOrder(
            ISaleOrderService _saleOrderService,
            [FromQuery] int saleOrderId
        )
        {
            var result = await _saleOrderService.DeleteSaleOrder(saleOrderId);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }
    }
}