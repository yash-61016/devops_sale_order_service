using System.Net;
using AutoMapper;
using devops_cart_service.Models;
using devops_sale_order_service.Models;
using devops_sale_order_service.Models.Dto;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Models.Dto.Get;
using devops_sale_order_service.Repository.IRepository;
using devops_sale_order_service.Service.IService;
using FluentValidation;

namespace devops_sale_order_service.Service
{
    public class SaleOrderService : ISaleOrderService
    {
        private readonly ISaleOrderRepository _saleOrderRepository;
        private readonly ISaleOrderLineRepository _saleOrderLineRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<SaleOrderCreateDto> _validator;

        public SaleOrderService(
            ISaleOrderRepository saleOrderRepository,
            ISaleOrderLineRepository saleOrderLineRepository,
            IMapper mapper,
            IValidator<SaleOrderCreateDto> validator
        )
        {
            _saleOrderRepository = saleOrderRepository;
            _saleOrderLineRepository = saleOrderLineRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<APIResponse> CreateSaleOrder(SaleOrderCreateDto saleOrderCreateDto)
        {
            var apiResponse = new APIResponse();
            var validationResult = await _validator.ValidateAsync(saleOrderCreateDto);
            if (!validationResult.IsValid)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

                return apiResponse;
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
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                apiResponse.ErrorMessages.Add(ex.Message);
                return apiResponse;
            };
        }

        public async Task<APIResponse> DeleteSaleOrder(int saleOrderId)
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

                    return apiResponse;
                }

                saleOrder.IsCancelled = true;
                await _saleOrderRepository.UpdateSaleOrder(saleOrder);

                apiResponse.Result = _mapper.Map<SaleOrderDto>(saleOrder);
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
                var saleOrders = await _saleOrderRepository.GetSaleOrdersByCustomerId(saleOrderGetQueryDto.UserId);

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
                    var saleOrderLines = await _saleOrderLineRepository.GetSaleOrderLinesBySaleOrderId(saleOrder.Id);
                    saleOrder.SaleOrderLines = _mapper.Map<List<SaleOrderLine>>(saleOrderLines);
                }

                apiResponse.Result = _mapper.Map<IEnumerable<SaleOrderDto>>(paginatedSaleOrders);
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
                var saleOrder = _mapper.Map<SaleOrder>(saleOrderUpdateDto);

                if (saleOrder == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = HttpStatusCode.NotFound;
                    apiResponse.ErrorMessages.Add($"SaleOrder with ID {saleOrderUpdateDto.Id} not found.");
                    return apiResponse;
                }

                await _saleOrderRepository.UpdateSaleOrder(saleOrder);
                apiResponse.Result = _mapper.Map<SaleOrderDto>(saleOrder);
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
            };
        }
    }
}