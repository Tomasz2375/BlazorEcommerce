using BlazorEcommerce.Server.Services.AuthenticationServices;
using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Server.Services.OrderService;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly ICartService cartService;
    private readonly IAuthenticationService authenticationService;
    private readonly IOrderService orderService;

    public PaymentService(
        ICartService cartService,
        IAuthenticationService authenticationService,
        IOrderService orderService)
    {
        StripeConfiguration.ApiKey = "secreatKey";
        this.cartService = cartService;
        this.authenticationService = authenticationService;
        this.orderService = orderService;
    }
    public async Task<Session> CreateChecoutSession()
    {
        var products = (await cartService.GetDbCartProducts()).Data;
        var lineItems = new List<SessionLineItemOptions>();
        products!.ForEach(product => lineItems.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions()
            {
                UnitAmountDecimal = product.Price * 100,
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = product.Title,
                    Images = new List<string> { product.ImageUrl },
                }
            },
            Quantity = product.Quantity,
        }));

        var options = new SessionCreateOptions()
        {
            CustomerEmail = authenticationService.GetUserEmail(),
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://localhost:7003/order-success",
            CancelUrl = "https://localhost:7003/cart",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return session;
    }
}
