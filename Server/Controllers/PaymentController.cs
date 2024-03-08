using BlazorEcommerce.Server.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        this.paymentService = paymentService;
    }

    [HttpPost("checkout"), Authorize]
    public async Task<ActionResult<string>> CreateCheckoutSession()
    {
        var session = await paymentService.CreateChecoutSession();

        return Ok(session.Url);
    }
}
