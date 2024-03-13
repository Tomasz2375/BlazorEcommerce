namespace BlazorEcommerce.Server.Services.AddressServices;

public interface IAddressService
{
    Task<ServiceResponse<Address>> GetAddress();
    Task<ServiceResponse<Address>> AddOrUpdate(Address address);
}
