using BlazorEcommerce.Server.Services.AuthenticationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
    {
        var response = await authenticationService.Login(request.Email, request.Password);

        if (!response.Sucess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("change-password"), Authorize]
    public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authenticationService
            .ChangePassword(int.Parse(userId), newPassword);

        if (!response.Sucess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

}
