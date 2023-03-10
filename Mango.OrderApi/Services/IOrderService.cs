using Commons;
using Mango.OrderApi.Dtos;

namespace Mango.OrderApi.Services;

public interface IOrderService
{
    Task<Result<object?>> CreateOrder(CheckoutDto checkout);

    Task<Result<object?>> UpdateOrderPaymentStatus(Guid orderHeaderId, bool paid);
}