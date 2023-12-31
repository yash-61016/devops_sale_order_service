using System.Net;
using AutoMapper;
using devops_cart_service.Models;
using devops_sale_order_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Models.Dto.Get;
using devops_sale_order_service.Service.IService;
using FluentValidation;

namespace devops_sale_order_service.Service
{
    public class SaleOrderServiceFake : ISaleOrderService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<SaleOrderCreateDto> _validator;

        public SaleOrderServiceFake(
            IMapper mapper,
            IValidator<SaleOrderCreateDto> validator
        )
        {
            _mapper = mapper;
            _validator = validator;
        }

        private SaleOrderDto[] _saleOrders =
        {
            new SaleOrderDto
            {
                Id = 1,
                CustomerId = 101,
                CustomerName = "John Doe",
                CustomerAddress = "123 Main St, Cityville",
                CustomerPhone = "+1234567890",
                CustomerEmail = "john.doe@example.com",
                TotalAmount = 250.00M,
                IsCancelled = false,
                SaleOrderLines = new List<SaleOrderLineDto>
                {
                    new SaleOrderLineDto
                    {
                        Id = 101,
                        ProductId = 201,
                        ProductName = "Product A",
                        Quantity = 2,
                        UnitPrice = 50.00M,
                        TotalAmount = 100.00M
                    },
                    new SaleOrderLineDto
                    {
                        Id = 102,
                        ProductId = 202,
                        ProductName = "Product B",
                        Quantity = 3,
                        UnitPrice = 30.00M,
                        TotalAmount = 90.00M
                    }
                }
            },
            new SaleOrderDto
            {
                Id = 2,
                CustomerId = 102,
                CustomerName = "Jane Smith",
                CustomerAddress = "456 Oak St, Townsville",
                CustomerPhone = "+9876543210",
                CustomerEmail = "jane.smith@example.com",
                TotalAmount = 180.50M,
                IsCancelled = false,
                SaleOrderLines = new List<SaleOrderLineDto>
                {
                    new SaleOrderLineDto
                    {
                        Id = 201,
                        ProductId = 203,
                        ProductName = "Product C",
                        Quantity = 1,
                        UnitPrice = 80.50M,
                        TotalAmount = 80.50M
                    },
                    new SaleOrderLineDto
                    {
                        Id = 202,
                        ProductId = 204,
                        ProductName = "Product D",
                        Quantity = 2,
                        UnitPrice = 50.00M,
                        TotalAmount = 100.00M
                    }
                }
            },
            new SaleOrderDto
            {
                Id = 3,
                CustomerId = 103,
                CustomerName = "Alice Johnson",
                CustomerAddress = "789 Pine St, Villagetown",
                CustomerPhone = "+1122334455",
                CustomerEmail = "alice.johnson@example.com",
                TotalAmount = 350.75M,
                IsCancelled = true,
                SaleOrderLines = new List<SaleOrderLineDto>
                {
                    new SaleOrderLineDto
                    {
                        Id = 301,
                        ProductId = 205,
                        ProductName = "Product E",
                        Quantity = 4,
                        UnitPrice = 25.00M,
                        TotalAmount = 100.00M
                    },
                    new SaleOrderLineDto
                    {
                        Id = 302,
                        ProductId = 206,
                        ProductName = "Product F",
                        Quantity = 5,
                        UnitPrice = 50.00M,
                        TotalAmount = 250.00M
                    }
                }
            },
        };

        public async Task<APIResponse> CreateSaleOrder(SaleOrderCreateDto saleOrderCreateDto)
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = _mapper.Map<SaleOrder>(saleOrderCreateDto);

                var validationResult = _validator.Validate(saleOrderCreateDto);

                if (!validationResult.IsValid)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.ErrorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return apiResponse;
                }

                saleOrder.Id = _saleOrders.Max(saleOrder => saleOrder.Id) + 1;
                saleOrder.IsCancelled = false;

                apiResponse.Result = saleOrder;
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.Created;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);

                return apiResponse;
            }
        }

        public async Task<APIResponse> DeleteSaleOrder(int saleOrderId)
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = _saleOrders.FirstOrDefault(saleOrder => saleOrder.Id == saleOrderId);

                if (saleOrder == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with ID {saleOrderId} not found.");
                    return apiResponse;
                }

                saleOrder.IsCancelled = true;

                apiResponse.Result = saleOrder;
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);

                return apiResponse;
            }
        }

        public async Task<APIResponse> GetSaleOrderByUserId(SaleOrderGetQueryDto saleOrderGetQueryDto)
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrders = _saleOrders.Where(saleOrder => saleOrder.CustomerId == saleOrderGetQueryDto.UserId).ToList();

                if (saleOrders == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with UserId {saleOrderGetQueryDto.UserId} not found.");
                    return apiResponse;
                }

                var paginatedSaleOrders = saleOrders.Skip(saleOrderGetQueryDto.Offset).Take(saleOrderGetQueryDto.Limit).ToList();

                foreach (var saleOrder in paginatedSaleOrders)
                {
                    var saleOrderLines = saleOrder.SaleOrderLines;
                    saleOrder.SaleOrderLines = saleOrderLines;
                }

                apiResponse.Result = paginatedSaleOrders;
                apiResponse.Result = saleOrders;
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);

                return apiResponse;
            }
        }

        public async Task<APIResponse> UpdateSaleOrder(SaleOrderDto saleOrderUpdateDto)
        {
            var apiResponse = new APIResponse();
            try
            {
                var saleOrder = _saleOrders.FirstOrDefault(saleOrder => saleOrder.Id == saleOrderUpdateDto.Id);

                if (saleOrder == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with ID {saleOrderUpdateDto.Id} not found.");
                    return apiResponse;
                }

                saleOrder.CustomerId = saleOrderUpdateDto.CustomerId;
                saleOrder.CustomerName = saleOrderUpdateDto.CustomerName;
                saleOrder.CustomerAddress = saleOrderUpdateDto.CustomerAddress;
                saleOrder.CustomerPhone = saleOrderUpdateDto.CustomerPhone;
                saleOrder.CustomerEmail = saleOrderUpdateDto.CustomerEmail;
                saleOrder.TotalAmount = saleOrderUpdateDto.TotalAmount;
                saleOrder.IsCancelled = saleOrderUpdateDto.IsCancelled;
                saleOrder.SaleOrderLines = saleOrderUpdateDto.SaleOrderLines;

                apiResponse.Result = saleOrder;
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = HttpStatusCode.OK;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);

                return apiResponse;
            }
        }
    }
}