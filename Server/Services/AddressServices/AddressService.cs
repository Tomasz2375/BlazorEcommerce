using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthenticationServices;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.AddressServices;

public class AddressService : IAddressService
{
    private readonly DataContext dataContext;
    private readonly IAuthenticationService authenticationService;

    public AddressService(
        DataContext dataContext,
        IAuthenticationService authenticationService)
    {
        this.dataContext = dataContext;
        this.authenticationService = authenticationService;
    }
    public async Task<ServiceResponse<Address>> AddOrUpdate(Address address)
    {
        var response = new ServiceResponse<Address>();
        var dbAddress = (await GetAddress()).Data;
        if (dbAddress is null)
        {
            address.UserId = authenticationService.GetUserId();
            dataContext.Addresses!.Add(address);
            response.Data = address;
        }
        else
        {
            dbAddress.FirstName = address.FirstName;
            dbAddress.LastName = address.LastName;
            dbAddress.Street = address.Street;
            dbAddress.Country = address.Country;
            dbAddress.City = address.City;
            dbAddress.Zip = address.Zip;
            dbAddress.State = address.State;

            response.Data = dbAddress;
        }

        await dataContext.SaveChangesAsync();

        return response;
    }

    public async Task<ServiceResponse<Address>> GetAddress()
    {
        int userId = authenticationService.GetUserId();

        var address = await dataContext
            .Addresses!
            .FirstOrDefaultAsync(x => x.UserId == userId);

        return new ServiceResponse<Address>
        {
            Sucess = true,
            Data = address
        };
    }
}
