using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService;

public interface IPaymentService
{
    Task<Session> CreateChecoutSession();
    Task<ServiceResponse<bool>> FulFillOrder(HttpRequest request);
}
