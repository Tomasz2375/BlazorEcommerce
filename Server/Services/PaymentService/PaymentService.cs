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

    private const string SECRET_KEY = "whsec_7d0403c3295273c038e20267a5e7aa01d6277708ac6e746c78c1ca33081871c4";

    public PaymentService(
        ICartService cartService,
        IAuthenticationService authenticationService,
        IOrderService orderService)
    {
        StripeConfiguration.ApiKey = "secretKey";
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

    public async Task<ServiceResponse<bool>> FulFillOrder(HttpRequest request)
    {
        var json = await new StreamReader(request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                request.Headers["Stripe-Signature"],
                SECRET_KEY);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                var user = await authenticationService.GetUserByEmail(session!.CustomerEmail);
                await orderService.PlaceOrder(user.Id);
            }

            return new ServiceResponse<bool>
            {
                Sucess = true,
                Data = true,
            };
        }
        catch(StripeException ex)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                Sucess = false,
                Message = ex.Message,
            };
        }
    }
}
