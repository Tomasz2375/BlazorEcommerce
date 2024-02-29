using BlazorEcommerce.Server.Services.AuthenticationServices;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
    {
        var respose = await authenticationService.Register(
            new User
            {
                Email = request.Email
            }, request.Password);

        if (!respose.Sucess)
        {
            return BadRequest(respose);
        }

        return Ok(respose);
    }
}
