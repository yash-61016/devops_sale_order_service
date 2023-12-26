using System.Net;
using AutoMapper;
using devops_cart_service.Models;
using devops_sale_order_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Repository.IRepository;
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

            app.MapGet("/api/saleorder/user-{{userId}}", GetSaleOrderByUserId)
            .WithName("GetSaleOrderByUserId")
            .Accepts<int>("application/json")
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
            ISaleOrderRepository _saleOrderRepository,
            IMapper _mapper,
            [FromBody] SaleOrderCreateDto saleOrderCreateDto
        )
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = _mapper.Map<SaleOrder>(saleOrderCreateDto);
                saleOrder.CreatedDate = DateTimeOffset.Now;
                saleOrder.UpdatedDate = DateTimeOffset.Now;
                await _saleOrderRepository.CreateSaleOrder(saleOrder);
                apiResponse.Result = _mapper.Map<SaleOrderDto>(saleOrder);
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.Created;
                return Results.Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);
                return Results.BadRequest(apiResponse);
            };
        }

        private async static Task<IResult> GetSaleOrderByUserId(
            ISaleOrderRepository _saleOrderRepository,
            IMapper _mapper,
            [FromQuery] int userId
        )
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrders = await _saleOrderRepository.GetSaleOrdersByCustomerId(userId);
                apiResponse.Result = _mapper.Map<IEnumerable<SaleOrderDto>>(saleOrders);
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return Results.Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);
                return Results.BadRequest(apiResponse);
            };
        }

        private async static Task<IResult> UpdateSaleOrder(
            ISaleOrderRepository _saleOrderRepository,
            IMapper _mapper,
            [FromBody] SaleOrderDto saleOrderUpdateDto
        )
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = _mapper.Map<SaleOrder>(saleOrderUpdateDto);
                await _saleOrderRepository.UpdateSaleOrder(saleOrder);
                apiResponse.Result = _mapper.Map<SaleOrderDto>(saleOrder);
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return Results.Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);
                return Results.BadRequest(apiResponse);
            };
        }

        private async static Task<IResult> DeleteSaleOrder(
            ISaleOrderRepository _saleOrderRepository,
            IMapper _mapper,
            [FromQuery] int saleOrderId
        )
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = await _saleOrderRepository.GetSaleOrderById(saleOrderId);
                saleOrder.IsCancelled = true;
                await _saleOrderRepository.UpdateSaleOrder(saleOrder);
                apiResponse.Result = _mapper.Map<SaleOrderDto>(saleOrder);
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return Results.Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);
                return Results.BadRequest(apiResponse);
            };
        }
    }
}