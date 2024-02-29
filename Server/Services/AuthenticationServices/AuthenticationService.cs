using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEcommerce.Server.Services.AuthenticationServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext dataContext;

    public AuthenticationService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }


    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>
        {
            Data = "token",
        };

        return response;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        if (await UserExist(user.Email))
        {
            return new ServiceResponse<int>()
            {
                Sucess = false,
                Message = "User already exists."
            };
        }

        createPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        dataContext.Users!.Add(user);
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<int>()
        {
            Data = user.Id,
            Message = "Registration successful!",
            Sucess = true,
        };
    }

    public async Task<bool> UserExist(string email) =>
        await dataContext
        .Users!
        .AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));

    private void createPasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
