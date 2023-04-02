using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public DiscountService(
        IDiscountRepository repository,
        ILogger logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName)
            ?? throw new RpcException(new Status(StatusCode.NotFound, "detail"));

        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _repository.CreateDiscount(coupon);

        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;

    }

    public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _repository.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        return _mapper.Map<CouponModel>(coupon);
    }

    public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);
        return new DeleteDiscountResponse
        {
            Success = deleted
        };
    }
}
