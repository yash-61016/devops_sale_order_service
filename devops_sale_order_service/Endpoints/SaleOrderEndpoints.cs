using System.Net;
using AutoMapper;
using devops_cart_service.Models;
using devops_sale_order_service.Filters;
using devops_sale_order_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Models.Dto.Get;
using devops_sale_order_service.Repository.IRepository;
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
            ISaleOrderRepository _saleOrderRepository,
            IMapper _mapper,
            IValidator<SaleOrderCreateDto> _validator,
            [FromBody] SaleOrderCreateDto saleOrderCreateDto
        )
        {
            var apiResponse = new APIResponse();
            var validationResult = await _validator.ValidateAsync(saleOrderCreateDto);
            if (!validationResult.IsValid)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

                return Results.BadRequest(apiResponse);
            }
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
            ISaleOrderLineRepository _saleOrderLineRepository,
            IMapper _mapper,
            [FromBody] SaleOrderGetQueryDto queryDto
        )
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrders = await _saleOrderRepository.GetSaleOrdersByCustomerId(queryDto.UserId);

                if (saleOrders == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with UserId {queryDto.UserId} not found.");
                    return Results.BadRequest(apiResponse);
                }

                var paginatedSaleOrders = saleOrders.Skip(queryDto.Offset).Take(queryDto.Limit).ToList();

                foreach (var saleOrder in paginatedSaleOrders)
                {
                    var saleOrderLines = await _saleOrderLineRepository.GetSaleOrderLinesBySaleOrderId(saleOrder.Id);
                    saleOrder.SaleOrderLines = _mapper.Map<List<SaleOrderLine>>(saleOrderLines);
                }

                apiResponse.Result = _mapper.Map<IEnumerable<SaleOrderDto>>(paginatedSaleOrders);
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
            }
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

                if (saleOrder == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with ID {saleOrderUpdateDto.Id} not found.");
                    return Results.BadRequest(apiResponse);
                }

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

                if (saleOrder == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with ID {saleOrderId} not found.");

                    return Results.BadRequest(apiResponse);
                }

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
            }
        }
    }
}