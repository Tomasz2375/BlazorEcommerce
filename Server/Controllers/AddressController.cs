using BlazorEcommerce.Server.Services.AddressServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AddressController
{
    private readonly IAddressService addressService;

    public AddressController(IAddressService addressService)
    {
        this.addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
    {
        return await addressService.GetAddress();
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
    {
        return await addressService.AddOrUpdate(address);
    }
}
